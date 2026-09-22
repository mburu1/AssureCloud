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

public class CorrectiveActionWorker : BackgroundService
{
    private readonly ILogger<CorrectiveActionWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public CorrectiveActionWorker(
        ILogger<CorrectiveActionWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableCorrectiveActionWorker)
        {
            _logger.LogInformation("Corrective Action Worker is disabled.");
            return;
        }

        _logger.LogInformation("Corrective Action Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckCorrectiveActionsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Corrective Action Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task CheckCorrectiveActionsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var now = DateTime.UtcNow;
        var sevenDaysFromNow = now.AddDays(7);
        var oneDayFromNow = now.AddDays(1);
        var yesterday = now.AddDays(-1);

        // Find corrective actions due soon
        var upcomingActions = await dbContext.CorrectiveActions
            .Where(c => c.Status == "InProgress" &&
                       c.DueDate != null &&
                       c.DueDate <= sevenDaysFromNow &&
                       c.DueDate > now)
            .ToListAsync(cancellationToken);

        foreach (var action in upcomingActions)
        {
            var daysUntilDue = (action.DueDate!.Value - now).Days;

            if (daysUntilDue == 7 || daysUntilDue == 1)
            {
                _logger.LogInformation("Sending reminder for corrective action {ActionId} due in {Days} days",
                    action.Id, daysUntilDue);

                if (action.AssignedToId != null)
                {
                    try
                    {
                        var user = await dbContext.Users.FindAsync(new object[] { action.AssignedToId }, cancellationToken);
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            await notificationService.SendEmailAsync(
                                user.Email,
                                $"Corrective Action Reminder: {action.Title}",
                                $"The corrective action \"{action.Title}\" is due in {daysUntilDue} day(s) on {action.DueDate:yyyy-MM-dd}.",
                                cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send reminder for corrective action {ActionId}", action.Id);
                    }
                }
            }
        }

        // Find overdue corrective actions
        var overdueActions = await dbContext.CorrectiveActions
            .Where(c => c.Status == "InProgress" &&
                       c.DueDate != null &&
                       c.DueDate < now)
            .ToListAsync(cancellationToken);

        foreach (var action in overdueActions)
        {
            _logger.LogWarning("Corrective action {ActionId} ({Title}) is overdue (due: {DueDate})",
                action.Id, action.Title, action.DueDate);

            if (action.AssignedToId != null)
            {
                try
                {
                    var user = await dbContext.Users.FindAsync(new object[] { action.AssignedToId }, cancellationToken);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await notificationService.SendEmailAsync(
                            user.Email,
                            $"Overdue Corrective Action: {action.Title}",
                            $"The corrective action \"{action.Title}\" was due on {action.DueDate:yyyy-MM-dd} but is still in progress.",
                            cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send overdue notification for corrective action {ActionId}", action.Id);
                }
            }
        }

        // Find corrective actions pending verification for a long time
        var pendingVerification = await dbContext.CorrectiveActions
            .Where(c => c.Status == "PendingVerification" &&
                       c.UpdatedAt != null &&
                       c.UpdatedAt <= yesterday)
            .ToListAsync(cancellationToken);

        foreach (var action in pendingVerification)
        {
            _logger.LogInformation("Corrective action {ActionId} ({Title}) pending verification since {UpdatedAt}",
                action.Id, action.Title, action.UpdatedAt);

            if (action.AssignedToId != null)
            {
                try
                {
                    var user = await dbContext.Users.FindAsync(new object[] { action.AssignedToId }, cancellationToken);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await notificationService.SendEmailAsync(
                            user.Email,
                            $"Corrective Action Awaiting Verification: {action.Title}",
                            $"The corrective action \"{action.Title}\" has been awaiting verification since {action.UpdatedAt:yyyy-MM-dd}.",
                            cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send pending verification notification for corrective action {ActionId}", action.Id);
                }
            }
        }
    }
}