using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Scope)
            .HasMaxLength(2000);

        builder.Property(a => a.Methodology)
            .HasMaxLength(2000);

        builder.Property(a => a.OverallScore)
            .HasPrecision(10, 2);

        builder.Property(a => a.MaxPossibleScore)
            .HasPrecision(10, 2);

        builder.Property(a => a.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(a => a.Program)
            .WithMany()
            .HasForeignKey(a => a.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Organization)
            .WithMany()
            .HasForeignKey(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Assessor)
            .WithMany()
            .HasForeignKey(a => a.AssessorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Responses)
            .WithOne()
            .HasForeignKey(r => r.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Evidence)
            .WithOne()
            .HasForeignKey(e => e.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Findings)
            .WithOne()
            .HasForeignKey(f => f.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Assignments)
            .WithOne()
            .HasForeignKey(a => a.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.ProgramId);
        builder.HasIndex(a => a.OrganizationId);
        builder.HasIndex(a => a.AssessorId);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => a.ScheduledStartDate);
    }
}

public class AssessmentResponseConfiguration : IEntityTypeConfiguration<AssessmentResponse>
{
    public void Configure(EntityTypeBuilder<AssessmentResponse> builder)
    {
        builder.ToTable("AssessmentResponses");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ResponseValue)
            .HasMaxLength(4000);

        builder.Property(r => r.Notes)
            .HasMaxLength(2000);

        builder.Property(r => r.Score)
            .HasPrecision(10, 2);

        builder.Property(r => r.MaxScore)
            .HasPrecision(10, 2);

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(r => r.Assessment)
            .WithMany(a => a.Responses)
            .HasForeignKey(r => r.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Criterion)
            .WithMany()
            .HasForeignKey(r => r.CriterionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Respondent)
            .WithMany()
            .HasForeignKey(r => r.RespondentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.AssessmentId);
        builder.HasIndex(r => r.CriterionId);
        builder.HasIndex(r => r.RespondentId);
        builder.HasIndex(r => r.Status);
    }
}

public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> builder)
    {
        builder.ToTable("Evidence");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.FileName)
            .HasMaxLength(500);

        builder.Property(e => e.FilePath)
            .HasMaxLength(1000);

        builder.Property(e => e.MimeType)
            .HasMaxLength(100);

        builder.Property(e => e.FileSize)
            .IsRequired();

        builder.Property(e => e.Hash)
            .HasMaxLength(500);

        builder.Property(e => e.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Source)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(e => e.Assessment)
            .WithMany(a => a.Evidence)
            .HasForeignKey(e => e.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Criterion)
            .WithMany()
            .HasForeignKey(e => e.CriterionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.UploadedBy)
            .WithMany()
            .HasForeignKey(e => e.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.AssessmentId);
        builder.HasIndex(e => e.CriterionId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.UploadedById);
    }
}

public class FindingConfiguration : IEntityTypeConfiguration<Finding>
{
    public void Configure(EntityTypeBuilder<Finding> builder)
    {
        builder.ToTable("Findings");

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

        builder.HasOne(f => f.Assessment)
            .WithMany(a => a.Findings)
            .HasForeignKey(f => f.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Criterion)
            .WithMany()
            .HasForeignKey(f => f.CriterionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.AssignedTo)
            .WithMany()
            .HasForeignKey(f => f.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => f.AssessmentId);
        builder.HasIndex(f => f.CriterionId);
        builder.HasIndex(f => f.Severity);
        builder.HasIndex(f => f.Status);
        builder.HasIndex(f => f.AssignedToId);
    }
}

public class AssessmentAssignmentConfiguration : IEntityTypeConfiguration<AssessmentAssignment>
{
    public void Configure(EntityTypeBuilder<AssessmentAssignment> builder)
    {
        builder.ToTable("AssessmentAssignments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(a => a.Assessment)
            .WithMany(a => a.Assignments)
            .HasForeignKey(a => a.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.AssessmentId);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.Status);
    }
}