using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssureCloud.Domain.Entities;
using AssureCloud.Infrastructure.Persistence;
using AssureCloud.Workers.WorkersOptions;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Workers.Workers;

public class DataRetentionWorker : BackgroundService
{
    private readonly ILogger<DataRetentionWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public DataRetentionWorker(
        ILogger<DataRetentionWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableDataRetentionWorker)
        {
            _logger.LogInformation("Data Retention Worker is disabled.");
            return;
        }

        _logger.LogInformation("Data Retention Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOldDataAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Data Retention Worker");
            }

            await Task.Delay(TimeSpan.FromDays(7), stoppingToken); // Run weekly
        }
    }

    private async Task CleanupOldDataAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();

        var retentionCutoff = DateTime.UtcNow.AddYears(-7); // 7 years retention

        _logger.LogInformation("Starting data retention cleanup for data older than {CutoffDate}", retentionCutoff);

        // Soft delete old completed assessments
        var oldAssessments = await dbContext.Assessments
            .Where(a => a.Status == "Completed" &&
                       a.CompletedAt != null &&
                       a.CompletedAt < retentionCutoff &&
                       !a.IsDeleted)
            .Take(100)
            .ToListAsync(cancellationToken);

        foreach (var assessment in oldAssessments)
        {
            assessment.MarkAsDeleted();
        }

        if (oldAssessments.Count > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Soft deleted {Count} old assessments", oldAssessments.Count);
        }

        // Soft delete old completed audits
        var oldAudits = await dbContext.Audits
            .Where(a => a.Status == "Completed" &&
                       a.CompletedAt != null &&
                       a.CompletedAt < retentionCutoff &&
                       !a.IsDeleted)
            .Take(100)
            .ToListAsync(cancellationToken);

        foreach (var audit in oldAudits)
        {
            audit.MarkAsDeleted();
        }

        if (oldAudits.Count > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Soft deleted {Count} old audits", oldAudits.Count);
        }

        // Clean up old domain events
        var oldDomainEvents = await dbContext.Set<DomainEvent>()
            .Where(e => e.CreatedAt < DateTime.UtcNow.AddDays(-90))
            .Take(1000)
            .ToListAsync(cancellationToken);

        if (oldDomainEvents.Count > 0)
        {
            dbContext.Set<DomainEvent>().RemoveRange(oldDomainEvents);
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Deleted {Count} old domain events", oldDomainEvents.Count);
        }

        // Clean up old notifications (if implemented)
        // var oldNotifications = await dbContext.Notifications...
    }
}