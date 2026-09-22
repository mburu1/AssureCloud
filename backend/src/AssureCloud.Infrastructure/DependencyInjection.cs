using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AssureCloud.Application.Abstractions;
using AssureCloud.Infrastructure.Persistence;
using AssureCloud.Infrastructure.Persistence.Repositories;
using AssureCloud.Infrastructure.Services;

namespace AssureCloud.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AssureCloudDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(AssureCloudDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IRepository<Organization>, OrganizationRepository>();
        services.AddScoped<IRepository<OrganizationLocation>, OrganizationLocationRepository>();
        services.AddScoped<IRepository<Supplier>, SupplierRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<IRepository<Program>, ProgramRepository>();
        services.AddScoped<IRepository<Standard>, StandardRepository>();
        services.AddScoped<IRepository<Requirement>, RequirementRepository>();
        services.AddScoped<IRepository<Criterion>, CriterionRepository>();
        services.AddScoped<IRepository<Control>, ControlRepository>();
        services.AddScoped<IRepository<Assessment>, AssessmentRepository>();
        services.AddScoped<IRepository<AssessmentResponse>, AssessmentResponseRepository>();
        services.AddScoped<IRepository<Evidence>, EvidenceRepository>();
        services.AddScoped<IRepository<Finding>, FindingRepository>();
        services.AddScoped<IRepository<AssessmentAssignment>, AssessmentAssignmentRepository>();
        services.AddScoped<IRepository<Audit>, AuditRepository>();
        services.AddScoped<IRepository<AuditFinding>, AuditFindingRepository>();
        services.AddScoped<IRepository<CorrectiveAction>, CorrectiveActionRepository>();
        services.AddScoped<IRepository<AuditAssignment>, AuditAssignmentRepository>();
        services.AddScoped<IRepository<Certification>, CertificationRepository>();
        services.AddScoped<IRepository<CertificationDecision>, CertificationDecisionRepository>();
        services.AddScoped<IRepository<CertificationScope>, CertificationScopeRepository>();
        services.AddScoped<IRepository<Report>, ReportRepository>();

        // Infrastructure Services
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}