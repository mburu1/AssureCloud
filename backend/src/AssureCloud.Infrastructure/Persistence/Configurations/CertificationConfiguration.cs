using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder.ToTable("Certifications");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CertificateNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Scope)
            .HasMaxLength(2000);

        builder.Property(c => c.StandardReference)
            .HasMaxLength(200);

        builder.Property(c => c.CertificationBody)
            .HasMaxLength(200);

        builder.Property(c => c.AccreditationBody)
            .HasMaxLength(200);

        builder.Property(c => c.Notes)
            .HasMaxLength(2000);

        builder.HasOne(c => c.Program)
            .WithMany()
            .HasForeignKey(c => c.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Organization)
            .WithMany()
            .HasForeignKey(c => c.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Audit)
            .WithMany()
            .HasForeignKey(c => c.AuditId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.IssuedBy)
            .WithMany()
            .HasForeignKey(c => c.IssuedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ApprovedBy)
            .WithMany()
            .HasForeignKey(c => c.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Decisions)
            .WithOne()
            .HasForeignKey(d => d.CertificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Scopes)
            .WithOne()
            .HasForeignKey(s => s.CertificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.ProgramId);
        builder.HasIndex(c => c.OrganizationId);
        builder.HasIndex(c => c.AuditId);
        builder.HasIndex(c => c.CertificateNumber).IsUnique();
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Type);
        builder.HasIndex(c => c.IssueDate);
        builder.HasIndex(c => c.ExpiryDate);
    }
}

public class CertificationDecisionConfiguration : IEntityTypeConfiguration<CertificationDecision>
{
    public void Configure(EntityTypeBuilder<CertificationDecision> builder)
    {
        builder.ToTable("CertificationDecisions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Decision)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Rationale)
            .HasMaxLength(3000);

        builder.Property(d => d.Conditions)
            .HasMaxLength(3000);

        builder.Property(d => d.DecidedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(d => d.Certification)
            .WithMany(c => c.Decisions)
            .HasForeignKey(d => d.CertificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.CertificationId);
        builder.HasIndex(d => d.Decision);
        builder.HasIndex(d => d.DecisionDate);
    }
}

public class CertificationScopeConfiguration : IEntityTypeConfiguration<CertificationScope>
{
    public void Configure(EntityTypeBuilder<CertificationScope> builder)
    {
        builder.ToTable("CertificationScopes");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(s => s.StandardClause)
            .HasMaxLength(200);

        builder.Property(s => s.Location)
            .HasMaxLength(500);

        builder.Property(s => s.ProcessArea)
            .HasMaxLength(500);

        builder.Property(s => s.Exclusions)
            .HasMaxLength(2000);

        builder.HasOne(s => s.Certification)
            .WithMany(c => c.Scopes)
            .HasForeignKey(s => s.CertificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.CertificationId);
    }
}