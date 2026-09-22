using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Aggregates;

#nullable disable

namespace AssureCloud.Infrastructure.Persistence.Context;

public class AssureCloudDbContext : IdentityDbContext<AssureCloudUser>
{
    public AssureCloudDbContext(DbContextOptions<AssureCloudDbContext> options)
        : base(options)
    {
    }

    // Organization
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationLocation> OrganizationLocations { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<User> Users { get; set; }

    // Programs
    public DbSet<Program> Programs { get; set; }
    public DbSet<Standard> Standards { get; set; }
    public DbSet<Requirement> Requirements { get; set; }
    public DbSet<Criterion> Criteria { get; set; }
    public DbSet<Control> Controls { get; set; }

    // Assessments
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<AssessmentResponse> AssessmentResponses { get; set; }
    public DbSet<Evidence> Evidence { get; set; }
    public DbSet<Finding> Findings { get; set; }
    public DbSet<AssessmentAssignment> AssessmentAssignments { get; set; }

    // Audits
    public DbSet<Audit> Audits { get; set; }
    public DbSet<AuditFinding> AuditFindings { get; set; }
    public DbSet<CorrectiveAction> CorrectiveActions { get; set; }
    public DbSet<AuditAssignment> AuditAssignments { get; set; }

    // Certifications
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<CertificationDecision> CertificationDecisions { get; set; }
    public DbSet<CertificationScope> CertificationScopes { get; set; }

    // Reports
    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AssureCloudDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }

            entity.UpdatedAt = now;
        }
    }
}
