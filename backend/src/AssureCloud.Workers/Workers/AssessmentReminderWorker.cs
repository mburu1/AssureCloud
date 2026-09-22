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

public class AssessmentReminderWorker : BackgroundService
{
    private readonly ILogger<AssessmentReminderWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public AssessmentReminderWorker(
        ILogger<AssessmentReminderWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableAssessmentReminderWorker)
        {
            _logger.LogInformation("Assessment Reminder Worker is disabled.");
            return;
        }

        _logger.LogInformation("Assessment Reminder Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendAssessmentRemindersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Assessment Reminder Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task SendAssessmentRemindersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var now = DateTime.UtcNow;
        var sevenDaysFromNow = now.AddDays(7);
        var oneDayFromNow = now.AddDays(1);

        // Find scheduled assessments starting soon
        var upcomingAssessments = await dbContext.Assessments
            .Where(a => a.Status == "Scheduled" &&
                       a.ScheduledStartDate != null &&
                       a.ScheduledStartDate <= sevenDaysFromNow &&
                       a.ScheduledStartDate > now)
            .ToListAsync(cancellationToken);

        foreach (var assessment in upcomingAssessments)
        {
            var daysUntilStart = (assessment.ScheduledStartDate!.Value - now).Days;

            if (daysUntilStart == 7 || daysUntilStart == 1)
            {
                _logger.LogInformation("Sending reminder for assessment {AssessmentId} starting in {Days} days",
                    assessment.Id, daysUntilStart);

                // Send notifications to assignees
                var assignments = await dbContext.AssessmentAssignments
                    .Where(a => a.AssessmentId == assessment.Id && a.Status == "Assigned")
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
                                $"Assessment Reminder: {assessment.Title}",
                                $"The assessment \"{assessment.Title}\" is scheduled to start in {daysUntilStart} day(s) on {assessment.ScheduledStartDate:yyyy-MM-dd}.",
                                cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send reminder for assessment {AssessmentId} to user {UserId}",
                            assessment.Id, assignment.UserId);
                    }
                }
            }
        }

        // Find in-progress assessments that might be overdue
        var overdueAssessments = await dbContext.Assessments
            .Where(a => a.Status == "InProgress" &&
                       a.ScheduledEndDate != null &&
                       a.ScheduledEndDate < now)
            .ToListAsync(cancellationToken);

        foreach (var assessment in overdueAssessments)
        {
            _logger.LogWarning("Assessment {AssessmentId} ({Title}) is overdue (scheduled end: {EndDate})",
                assessment.Id, assessment.Title, assessment.ScheduledEndDate);

            var assignments = await dbContext.AssessmentAssignments
                .Where(a => a.AssessmentId == assessment.Id && a.Status == "Assigned")
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
                            $"Overdue Assessment: {assessment.Title}",
                            $"The assessment \"{assessment.Title}\" was scheduled to end on {assessment.ScheduledEndDate:yyyy-MM-dd} but is still in progress.",
                            cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send overdue notification for assessment {AssessmentId} to user {UserId}",
                        assessment.Id, assignment.UserId);
                }
            }
        }
    }
}