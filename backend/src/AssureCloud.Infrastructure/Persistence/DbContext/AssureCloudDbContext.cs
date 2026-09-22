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

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Program> Programs { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<Audit> Audits { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Evidence> Evidence { get; set; }
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
