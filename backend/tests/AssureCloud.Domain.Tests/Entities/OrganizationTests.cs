using System;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.ValueObjects;
using Xunit;

namespace AssureCloud.Domain.Tests.Entities;

public class OrganizationTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOrganization()
    {
        // Arrange
        var name = "Test Organization";
        var description = "Test Description";
        var registrationNumber = "REG-001";
        var createdBy = "test-user";

        // Act
        var organization = Organization.Create(name, description, registrationNumber, createdBy);

        // Assert
        Assert.NotNull(organization);
        Assert.Equal(name, organization.Name);
        Assert.Equal(description, organization.Description);
        Assert.Equal(registrationNumber, organization.RegistrationNumber);
        Assert.Equal(OrganizationStatus.Active, organization.Status);
        Assert.NotEqual(Guid.Empty, organization.Id);
        Assert.True(organization.CreatedAt <= DateTime.UtcNow);
        Assert.Equal(createdBy, organization.CreatedBy);
    }

    [Fact]
    public void Create_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var name = (string?)null;
        var createdBy = "test-user";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Organization.Create(name!, "Description", "REG-001", createdBy));
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "";
        var createdBy = "test-user";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Organization.Create(name, "Description", "REG-001", createdBy));
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateOrganization()
    {
        // Arrange
        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        var newName = "Updated Organization";
        var newDescription = "Updated Description";
        var updatedBy = "updated-user";

        // Act
        organization.Update(newName, newDescription, "REG-002", "TAX-001", "https://example.com", "https://logo.com", updatedBy);

        // Assert
        Assert.Equal(newName, organization.Name);
        Assert.Equal(newDescription, organization.Description);
        Assert.Equal("REG-002", organization.RegistrationNumber);
        Assert.Equal("TAX-001", organization.TaxId);
        Assert.Equal("https://example.com", organization.Website);
        Assert.Equal("https://logo.com", organization.LogoUrl);
        Assert.Equal(updatedBy, organization.UpdatedBy);
    }

    [Fact]
    public void ChangeStatus_ShouldUpdateStatusAndRaiseEvent()
    {
        // Arrange
        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        organization.ClearDomainEvents();

        // Act
        organization.ChangeStatus(OrganizationStatus.Inactive, "admin-user");

        // Assert
        Assert.Equal(OrganizationStatus.Inactive, organization.Status);
        Assert.Equal("admin-user", organization.UpdatedBy);
        Assert.Single(organization.DomainEvents);
    }

    [Fact]
    public void AddLocation_ShouldAddLocationToOrganization()
    {
        // Arrange
        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        organization.ClearDomainEvents();

        var address = Address.Create("123 Main St", "City", "State", "Country", "12345");
        var locationName = "Headquarters";

        // Act
        var location = organization.AddLocation(locationName, address, "John Doe", "john@example.com", "+1234567890", "UTC", true, "test-user");

        // Assert
        Assert.NotNull(location);
        Assert.Equal(locationName, location.Name);
        Assert.Equal(address, location.Address);
        Assert.True(location.IsPrimary);
        Assert.Single(organization.Locations);
        Assert.Single(organization.DomainEvents);
    }

    [Fact]
    public void AddSupplier_ShouldAddSupplierToOrganization()
    {
        // Arrange
        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        organization.ClearDomainEvents();

        var email = Email.Create("supplier@example.com");
        var phone = PhoneNumber.Create("+1234567890");
        var address = Address.Create("456 Supplier St", "City", "State", "Country", "12345");

        // Act
        var supplier = organization.AddSupplier("Test Supplier", email, phone, address, "TAX-001", "SUP-001", "https://supplier.com", "test-user");

        // Assert
        Assert.NotNull(supplier);
        Assert.Equal("Test Supplier", supplier.Name);
        Assert.Equal(email, supplier.ContactEmail);
        Assert.Equal(phone, supplier.ContactPhone);
        Assert.Equal(SupplierStatus.Active, supplier.Status);
        Assert.Single(organization.Suppliers);
        Assert.Single(organization.DomainEvents);
    }

    [Fact]
    public void AddUser_ShouldAddUserToOrganization()
    {
        // Arrange
        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        organization.ClearDomainEvents();

        var email = Email.Create("user@example.com");
        var phone = PhoneNumber.Create("+1234567890");

        // Act
        var user = organization.AddUser(email, "John", "Doe", phone, "UTC", "en", UserRole.Auditor, "admin-user");

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal(UserRole.Auditor, user.Role);
        Assert.Equal(UserStatus.Active, user.Status);
        Assert.Single(organization.Users);
        Assert.Single(organization.DomainEvents);
    }
}