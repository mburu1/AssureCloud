using System;
using AssureCloud.Domain.Entities;
using Xunit;

namespace AssureCloud.Domain.Tests.Entities;

public class ProgramTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateProgram()
    {
        // Arrange
        var name = "ISO 27001 Program";
        var description = "Information Security Management Program";
        var code = "ISO27001";
        var version = "2022";
        var organizationId = Guid.NewGuid();
        var createdBy = "test-user";

        // Act
        var program = Program.Create(name, description, code, version, organizationId, createdBy);

        // Assert
        Assert.NotNull(program);
        Assert.Equal(name, program.Name);
        Assert.Equal(description, program.Description);
        Assert.Equal(code, program.Code);
        Assert.Equal(version, program.Version);
        Assert.Equal(organizationId, program.OrganizationId);
        Assert.Equal(ProgramStatus.Draft, program.Status);
        Assert.NotEqual(Guid.Empty, program.Id);
        Assert.Equal(createdBy, program.CreatedBy);
    }

    [Fact]
    public void Create_WithEmptyCode_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "Test Program";
        var code = "";
        var organizationId = Guid.NewGuid();
        var createdBy = "test-user";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Program.Create(name, "Description", code, "1.0", organizationId, createdBy));
    }

    [Fact]
    public void AddStandard_ShouldAddStandardToProgram()
    {
        // Arrange
        var program = Program.Create("Test Program", "Description", "TP", "1.0", Guid.NewGuid(), "test-user");
        program.ClearDomainEvents();

        // Act
        var standard = program.AddStandard("ISO 27001", "ISO27001", "2022", "Information Security Standard", "https://iso.org/27001", "test-user");

        // Assert
        Assert.NotNull(standard);
        Assert.Equal("ISO 27001", standard.Name);
        Assert.Equal("ISO27001", standard.Code);
        Assert.Equal("2022", standard.Version);
        Assert.Equal(StandardStatus.Draft, standard.Status);
        Assert.Single(program.Standards);
        Assert.Single(program.DomainEvents);
    }

    [Fact]
    public void ChangeStatus_ShouldUpdateStatusAndRaiseEvent()
    {
        // Arrange
        var program = Program.Create("Test Program", "Description", "TP", "1.0", Guid.NewGuid(), "test-user");
        program.ClearDomainEvents();

        // Act
        program.ChangeStatus(ProgramStatus.Active, "admin-user");

        // Assert
        Assert.Equal(ProgramStatus.Active, program.Status);
        Assert.Equal("admin-user", program.UpdatedBy);
        Assert.Single(program.DomainEvents);
    }
}