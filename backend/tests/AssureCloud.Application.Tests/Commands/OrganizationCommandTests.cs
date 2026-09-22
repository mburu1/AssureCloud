using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Organizations;
using AssureCloud.Application.DTOs.Organizations;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace AssureCloud.Application.Tests.Commands;

public class CreateOrganizationCommandValidatorTests
{
    private readonly CreateOrganizationCommandValidator _validator;

    public CreateOrganizationCommandValidatorTests()
    {
        _validator = new CreateOrganizationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateOrganizationCommand { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var command = new CreateOrganizationCommand { Name = null! };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new CreateOrganizationCommand { Name = new string('a', 201) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var command = new CreateOrganizationCommand { Name = "Valid Organization" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_RegistrationNumber_Exceeds_MaxLength()
    {
        var command = new CreateOrganizationCommand
        {
            Name = "Valid Org",
            RegistrationNumber = new string('a', 101)
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RegistrationNumber);
    }
}

public class UpdateOrganizationCommandValidatorTests
{
    private readonly UpdateOrganizationCommandValidator _validator;

    public UpdateOrganizationCommandValidatorTests()
    {
        _validator = new UpdateOrganizationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateOrganizationCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateOrganizationCommand { Id = Guid.NewGuid(), Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}

public class CreateOrganizationCommandHandlerTests
{
    private readonly Mock<IRepository<Organization>> _organizationRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateOrganizationCommandHandler _handler;

    public CreateOrganizationCommandHandlerTests()
    {
        _organizationRepositoryMock = new Mock<IRepository<Organization>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateOrganizationCommandHandler(_organizationRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateOrganization()
    {
        // Arrange
        var command = new CreateOrganizationCommand
        {
            Name = "Test Organization",
            Description = "Test Description",
            RegistrationNumber = "REG-001",
            CreatedBy = "test-user"
        };

        Organization? capturedOrganization = null;
        _organizationRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Organization>(), It.IsAny<CancellationToken>()))
            .Callback<Organization, CancellationToken>((org, _) => capturedOrganization = org)
            .ReturnsAsync((Organization org, CancellationToken _) => org);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.RegistrationNumber, result.RegistrationNumber);
        Assert.Equal(OrganizationStatus.Active, result.Status);

        _organizationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Organization>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}