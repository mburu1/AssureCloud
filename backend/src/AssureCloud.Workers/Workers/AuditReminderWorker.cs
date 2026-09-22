using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using AssureCloud.Infrastructure.Persistence;
using AssureCloud.Workers.WorkersOptions;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Workers.Workers;

public class AuditReminderWorker : BackgroundService
{
    private readonly ILogger<AuditReminderWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public AuditReminderWorker(
        ILogger<AuditReminderWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableAuditReminderWorker)
        {
            _logger.LogInformation("Audit Reminder Worker is disabled.");
            return;
        }

        _logger.LogInformation("Audit Reminder Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendAuditRemindersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Audit Reminder Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task SendAuditRemindersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var now = DateTime.UtcNow;
        var sevenDaysFromNow = now.AddDays(7);
        var oneDayFromNow = now.AddDays(1);

        // Find scheduled audits starting soon
        var upcomingAudits = await dbContext.Audits
            .Where(a => a.Status == "Scheduled" &&
                       a.ScheduledStartDate != null &&
                       a.ScheduledStartDate <= sevenDaysFromNow &&
                       a.ScheduledStartDate > now)
            .ToListAsync(cancellationToken);

        foreach (var audit in upcomingAudits)
        {
            var daysUntilStart = (audit.ScheduledStartDate!.Value - now).Days;

            if (daysUntilStart == 7 || daysUntilStart == 1)
            {
                _logger.LogInformation("Sending reminder for audit {AuditId} starting in {Days} days",
                    audit.Id, daysUntilStart);

                // Send notifications to assignees
                var assignments = await dbContext.AuditAssignments
                    .Where(a => a.AuditId == audit.Id && a.Status == "Assigned")
                    .ToListAsync(cancellationToken);

                foreach (var assignment in assignments)
                {
                    try
                    {
                        var user = await dbContext.Users.FindAsync(new object[] { assignment.UserId }, cancellationToken);
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            await notificationService.SendEmailAsync(
                                user.Email,
                                $"Audit Reminder: {audit.Title}",
                                $"The audit \"{audit.Title}\" is scheduled to start in {daysUntilStart} day(s) on {audit.ScheduledStartDate:yyyy-MM-dd}.",
                                cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send reminder for audit {AuditId} to user {UserId}",
                            audit.Id, assignment.UserId);
                    }
                }
            }
        }

        // Find in-progress audits that might be overdue
        var overdueAudits = await dbContext.Audits
            .Where(a => a.Status == "InProgress" &&
                       a.ScheduledEndDate != null &&
                       a.ScheduledEndDate < now)
            .ToListAsync(cancellationToken);

        foreach (var audit in overdueAudits)
        {
            _logger.LogWarning("Audit {AuditId} ({Title}) is overdue (scheduled end: {EndDate})",
                audit.Id, audit.Title, audit.ScheduledEndDate);

            var assignments = await dbContext.AuditAssignments
                .Where(a => a.AuditId == audit.Id && a.Status == "Assigned")
                .ToListAsync(cancellationToken);

            foreach (var assignment in assignments)
            {
                try
                {
                    var user = await dbContext.Users.FindAsync(new object[] { assignment.UserId }, cancellationToken);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await notificationService.SendEmailAsync(
                            user.Email,
                            $"Overdue Audit: {audit.Title}",
                            $"The audit \"{audit.Title}\" was scheduled to end on {audit.ScheduledEndDate:yyyy-MM-dd} but is still in progress.",
                            cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send overdue notification for audit {AuditId} to user {UserId}",
                        audit.Id, assignment.UserId);
                }
            }
        }
    }
}