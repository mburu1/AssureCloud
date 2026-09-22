using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Certifications;
using AssureCloud.Domain.Entities;
using AssureCloud.Infrastructure.Persistence;
using AssureCloud.Workers.WorkersOptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Workers.Workers;

public class CertificationExpiryWorker : BackgroundService
{
    private readonly ILogger<CertificationExpiryWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public CertificationExpiryWorker(
        ILogger<CertificationExpiryWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableCertificationExpiryWorker)
        {
            _logger.LogInformation("Certification Expiry Worker is disabled.");
            return;
        }

        _logger.LogInformation("Certification Expiry Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiringCertificationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Certification Expiry Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task CheckExpiringCertificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();

        var now = DateTime.UtcNow;
        var thirtyDaysFromNow = now.AddDays(30);
        var sevenDaysFromNow = now.AddDays(7);
        var oneDayFromNow = now.AddDays(1);

        // Find certifications expiring soon
        var expiringCertifications = await dbContext.Certifications
            .Where(c => c.Status == "Active" &&
                       c.ExpiryDate != null &&
                       c.ExpiryDate <= thirtyDaysFromNow &&
                       c.ExpiryDate > now)
            .ToListAsync(cancellationToken);

        foreach (var certification in expiringCertifications)
        {
            var daysUntilExpiry = (certification.ExpiryDate!.Value - now).Days;

            _logger.LogInformation("Certification {CertificationId} ({CertificateNumber}) expires in {Days} days",
                certification.Id, certification.CertificateNumber, daysUntilExpiry);

            // Send notifications at 30, 7, and 1 day intervals
            if (daysUntilExpiry == 30 || daysUntilExpiry == 7 || daysUntilExpiry == 1)
            {
                // TODO: Send notification via notification service
                _logger.LogInformation("Sending expiry notification for certification {CertificationId}", certification.Id);
            }
        }

        // Find certifications that have expired
        var expiredCertifications = await dbContext.Certifications
            .Where(c => c.Status == "Active" &&
                       c.ExpiryDate != null &&
                       c.ExpiryDate <= now)
            .ToListAsync(cancellationToken);

        foreach (var certification in expiredCertifications)
        {
            try
            {
                _logger.LogInformation("Certification {CertificationId} ({CertificateNumber}) has expired, updating status",
                    certification.Id, certification.CertificateNumber);

                var command = new UpdateCertificationCommand
                {
                    Id = certification.Id,
                    CertificateNumber = certification.CertificateNumber,
                    ProgramId = certification.ProgramId,
                    OrganizationId = certification.OrganizationId,
                    AuditId = certification.AuditId,
                    Type = certification.Type,
                    Status = "Expired",
                    Scope = certification.Scope,
                    StandardReference = certification.StandardReference,
                    CertificationBody = certification.CertificationBody,
                    AccreditationBody = certification.AccreditationBody,
                    IssueDate = certification.IssueDate,
                    ExpiryDate = certification.ExpiryDate,
                    Notes = certification.Notes
                };

                await mediator.Send(command, cancellationToken);

                _logger.LogInformation("Certification {CertificationId} status updated to Expired", certification.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update expired certification {CertificationId}", certification.Id);
            }
        }
    }
}