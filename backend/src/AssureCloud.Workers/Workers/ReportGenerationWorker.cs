using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Reports;
using AssureCloud.Application.DTOs.Reports;
using AssureCloud.Application.Queries.Reports;
using AssureCloud.Infrastructure.Persistence;
using AssureCloud.Workers.WorkersOptions;
using MediatR;

namespace AssureCloud.Workers.Workers;

public class ReportGenerationWorker : BackgroundService
{
    private readonly ILogger<ReportGenerationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WorkersOptions _options;

    public ReportGenerationWorker(
        ILogger<ReportGenerationWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkersOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableReportGenerationWorker)
        {
            _logger.LogInformation("Report Generation Worker is disabled.");
            return;
        }

        _logger.LogInformation("Report Generation Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateScheduledReportsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Report Generation Worker");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task GenerateScheduledReportsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssureCloudDbContext>();

        // Find reports that need to be generated
        var pendingReports = await dbContext.Reports
            .Where(r => r.Status == "Pending" && r.ScheduledGenerationAt != null && r.ScheduledGenerationAt <= DateTime.UtcNow)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var report in pendingReports)
        {
            try
            {
                _logger.LogInformation("Generating report {ReportId}: {ReportTitle}", report.Id, report.Title);

                var command = new GenerateReportCommand(report.Id);
                await mediator.Send(command, cancellationToken);

                _logger.LogInformation("Report {ReportId} generated successfully", report.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate report {ReportId}", report.Id);
            }
        }
    }
}