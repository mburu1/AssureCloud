using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable("Audits");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.Scope)
            .HasMaxLength(2000);

        builder.Property(a => a.Criteria)
            .HasMaxLength(2000);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.OverallRating)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.ReportReference)
            .HasMaxLength(200);

        builder.HasOne(a => a.Program)
            .WithMany()
            .HasForeignKey(a => a.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Organization)
            .WithMany()
            .HasForeignKey(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.LeadAuditor)
            .WithMany()
            .HasForeignKey(a => a.LeadAuditorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Findings)
            .WithOne()
            .HasForeignKey(f => f.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Assignments)
            .WithOne()
            .HasForeignKey(a => a.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.CorrectiveActions)
            .WithOne()
            .HasForeignKey(c => c.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.ProgramId);
        builder.HasIndex(a => a.OrganizationId);
        builder.HasIndex(a => a.LeadAuditorId);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => a.Type);
        builder.HasIndex(a => a.ScheduledStartDate);
    }
}

public class AuditFindingConfiguration : IEntityTypeConfiguration<AuditFinding>
{
    public void Configure(EntityTypeBuilder<AuditFinding> builder)
    {
        builder.ToTable("AuditFindings");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Description)
            .HasMaxLength(3000);

        builder.Property(f => f.Recommendation)
            .HasMaxLength(3000);

        builder.Property(f => f.Severity)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(f => f.RequirementReference)
            .HasMaxLength(200);

        builder.HasOne(f => f.Audit)
            .WithMany(a => a.Findings)
            .HasForeignKey(f => f.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.CorrectiveAction)
            .WithOne()
            .HasForeignKey<AuditFinding>(f => f.CorrectiveActionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(f => f.AssignedTo)
            .WithMany()
            .HasForeignKey(f => f.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => f.AuditId);
        builder.HasIndex(f => f.Severity);
        builder.HasIndex(f => f.Status);
        builder.HasIndex(f => f.AssignedToId);
    }
}

public class CorrectiveActionConfiguration : IEntityTypeConfiguration<CorrectiveAction>
{
    public void Configure(EntityTypeBuilder<CorrectiveAction> builder)
    {
        builder.ToTable("CorrectiveActions");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(3000);

        builder.Property(c => c.RootCause)
            .HasMaxLength(3000);

        builder.Property(c => c.ProposedAction)
            .HasMaxLength(3000);

        builder.Property(c => c.ImplementedAction)
            .HasMaxLength(3000);

        builder.Property(c => c.EffectivenessReview)
            .HasMaxLength(3000);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Priority)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ReferenceNumber)
            .HasMaxLength(100);

        builder.HasOne(c => c.Audit)
            .WithMany(a => a.CorrectiveActions)
            .HasForeignKey(c => c.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Finding)
            .WithMany()
            .HasForeignKey(c => c.FindingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.AssignedTo)
            .WithMany()
            .HasForeignKey(c => c.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.VerifiedBy)
            .WithMany()
            .HasForeignKey(c => c.VerifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.AuditId);
        builder.HasIndex(c => c.FindingId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Priority);
        builder.HasIndex(c => c.AssignedToId);
    }
}

public class AuditAssignmentConfiguration : IEntityTypeConfiguration<AuditAssignment>
{
    public void Configure(EntityTypeBuilder<AuditAssignment> builder)
    {
        builder.ToTable("AuditAssignments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(a => a.Audit)
            .WithMany(a => a.Assignments)
            .HasForeignKey(a => a.AuditId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.AuditId);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.Status);
    }
}