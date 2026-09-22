using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("Reports");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .HasMaxLength(2000);

        builder.Property(r => r.ReportNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Format)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Content)
            .HasColumnType("nvarchar(max)");

        builder.Property(r => r.TemplateName)
            .HasMaxLength(200);

        builder.Property(r => r.GeneratedBy)
            .HasMaxLength(200);

        builder.Property(r => r.ReviewedBy)
            .HasMaxLength(200);

        builder.Property(r => r.ApprovedBy)
            .HasMaxLength(200);

        builder.Property(r => r.FilePath)
            .HasMaxLength(1000);

        builder.Property(r => r.FileSize)
            .IsRequired(false);

        builder.Property(r => r.MimeType)
            .HasMaxLength(100);

        builder.HasOne(r => r.Organization)
            .WithMany()
            .HasForeignKey(r => r.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Program)
            .WithMany()
            .HasForeignKey(r => r.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Assessment)
            .WithMany()
            .HasForeignKey(r => r.AssessmentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.Audit)
            .WithMany()
            .HasForeignKey(r => r.AuditId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.Certification)
            .WithMany()
            .HasForeignKey(r => r.CertificationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(r => r.OrganizationId);
        builder.HasIndex(r => r.ProgramId);
        builder.HasIndex(r => r.AssessmentId);
        builder.HasIndex(r => r.AuditId);
        builder.HasIndex(r => r.CertificationId);
        builder.HasIndex(r => r.ReportNumber).IsUnique();
        builder.HasIndex(r => r.Type);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.GeneratedAt);
    }
}