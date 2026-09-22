using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class ProgramConfiguration : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> builder)
    {
        builder.ToTable("Programs");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(p => p.Organization)
            .WithMany()
            .HasForeignKey(p => p.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Standards)
            .WithOne()
            .HasForeignKey(s => s.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.OrganizationId);
        builder.HasIndex(p => p.Code).IsUnique();
        builder.HasIndex(p => p.Status);
    }
}

public class StandardConfiguration : IEntityTypeConfiguration<Standard>
{
    public void Configure(EntityTypeBuilder<Standard> builder)
    {
        builder.ToTable("Standards");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Version)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Description)
            .HasMaxLength(2000);

        builder.Property(s => s.ExternalReferenceUrl)
            .HasMaxLength(500);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(s => s.Program)
            .WithMany(p => p.Standards)
            .HasForeignKey(s => s.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Requirements)
            .WithOne()
            .HasForeignKey(r => r.StandardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProgramId);
        builder.HasIndex(s => s.Code);
        builder.HasIndex(s => s.Status);
    }
}

public class RequirementConfiguration : IEntityTypeConfiguration<Requirement>
{
    public void Configure(EntityTypeBuilder<Requirement> builder)
    {
        builder.ToTable("Requirements");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.Description)
            .HasMaxLength(3000);

        builder.Property(r => r.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(r => r.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(r => r.Category)
            .HasMaxLength(200);

        builder.Property(r => r.Priority)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(r => r.Standard)
            .WithMany(s => s.Requirements)
            .HasForeignKey(r => r.StandardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Criteria)
            .WithOne()
            .HasForeignKey(c => c.RequirementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.StandardId);
        builder.HasIndex(r => r.Code);
        builder.HasIndex(r => r.Priority);
    }
}

public class CriterionConfiguration : IEntityTypeConfiguration<Criterion>
{
    public void Configure(EntityTypeBuilder<Criterion> builder)
    {
        builder.ToTable("Criteria");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Description)
            .HasMaxLength(3000);

        builder.Property(c => c.Guidance)
            .HasMaxLength(3000);

        builder.Property(c => c.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ResponseType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ScoringMethod)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.MinScore)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(c => c.MaxScore)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.HasOne(c => c.Requirement)
            .WithMany(r => r.Criteria)
            .HasForeignKey(c => c.RequirementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Controls)
            .WithOne()
            .HasForeignKey(c => c.CriterionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.RequirementId);
        builder.HasIndex(c => c.Code);
        builder.HasIndex(c => c.Type);
    }
}

public class ControlConfiguration : IEntityTypeConfiguration<Control>
{
    public void Configure(EntityTypeBuilder<Control> builder)
    {
        builder.ToTable("Controls");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Description)
            .HasMaxLength(3000);

        builder.Property(c => c.ImplementationGuidance)
            .HasMaxLength(3000);

        builder.Property(c => c.TestingGuidance)
            .HasMaxLength(3000);

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Frequency)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.AutomationLevel)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(c => c.Criterion)
            .WithMany(c => c.Controls)
            .HasForeignKey(c => c.CriterionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.CriterionId);
        builder.HasIndex(c => c.Code);
        builder.HasIndex(c => c.Type);
    }
}