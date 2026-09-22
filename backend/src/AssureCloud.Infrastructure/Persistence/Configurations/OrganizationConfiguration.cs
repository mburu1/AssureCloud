using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AssureCloud.Domain.Entities;

namespace AssureCloud.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Description)
            .HasMaxLength(1000);

        builder.Property(o => o.RegistrationNumber)
            .HasMaxLength(100);

        builder.Property(o => o.TaxId)
            .HasMaxLength(100);

        builder.Property(o => o.Website)
            .HasMaxLength(500);

        builder.Property(o => o.LogoUrl)
            .HasMaxLength(500);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(o => o.ParentOrganization)
            .WithMany()
            .HasForeignKey(o => o.ParentOrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Locations)
            .WithOne()
            .HasForeignKey(l => l.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Suppliers)
            .WithOne()
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Users)
            .WithOne()
            .HasForeignKey(u => u.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.Name);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.ParentOrganizationId);
    }
}

public class OrganizationLocationConfiguration : IEntityTypeConfiguration<OrganizationLocation>
{
    public void Configure(EntityTypeBuilder<OrganizationLocation> builder)
    {
        builder.ToTable("OrganizationLocations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(l => l.City)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.State)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Country)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.PostalCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.ContactName)
            .HasMaxLength(200);

        builder.Property(l => l.ContactEmail)
            .HasMaxLength(200);

        builder.Property(l => l.ContactPhone)
            .HasMaxLength(50);

        builder.Property(l => l.Timezone)
            .HasMaxLength(100);

        builder.HasOne(l => l.Organization)
            .WithMany(o => o.Locations)
            .HasForeignKey(l => l.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.OrganizationId);
        builder.HasIndex(l => l.IsPrimary);
    }
}

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ContactEmail)
            .HasMaxLength(200);

        builder.Property(s => s.ContactPhone)
            .HasMaxLength(50);

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.TaxId)
            .HasMaxLength(100);

        builder.Property(s => s.RegistrationNumber)
            .HasMaxLength(100);

        builder.Property(s => s.Website)
            .HasMaxLength(500);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(s => s.Organization)
            .WithMany(o => o.Suppliers)
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.OrganizationId);
        builder.HasIndex(s => s.Status);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(u => u.Timezone)
            .HasMaxLength(100);

        builder.Property(u => u.Language)
            .HasMaxLength(50);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(u => u.Organization)
            .WithMany(o => o.Users)
            .HasForeignKey(u => u.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => u.OrganizationId);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Status);
    }
}