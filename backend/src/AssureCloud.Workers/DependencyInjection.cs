using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AssureCloud.Workers.Workers;

namespace AssureCloud.Workers;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WorkersOptions>(configuration.GetSection(WorkersOptions.SectionName));

        services.AddHostedService<ReportGenerationWorker>();
        services.AddHostedService<CertificationExpiryWorker>();
        services.AddHostedService<AssessmentReminderWorker>();
        services.AddHostedService<AuditReminderWorker>();
        services.AddHostedService<CorrectiveActionWorker>();
        services.AddHostedService<DataRetentionWorker>();

        return services;
    }
}