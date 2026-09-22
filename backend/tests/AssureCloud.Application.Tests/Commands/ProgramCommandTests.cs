using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Programs;
using AssureCloud.Application.DTOs.Programs;
using AssureCloud.Domain.Entities;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace AssureCloud.Application.Tests.Commands;

public class CreateProgramCommandValidatorTests
{
    private readonly CreateProgramCommandValidator _validator;

    public CreateProgramCommandValidatorTests()
    {
        _validator = new CreateProgramCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateProgramCommand { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var command = new CreateProgramCommand { Name = "Valid Program", Code = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_Code_Exceeds_MaxLength()
    {
        var command = new CreateProgramCommand { Name = "Valid Program", Code = new string('a', 51) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Should_Have_Error_When_OrganizationId_Is_Empty()
    {
        var command = new CreateProgramCommand { Name = "Valid Program", Code = "VP", OrganizationId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateProgramCommand
        {
            Name = "Valid Program",
            Code = "VP",
            Version = "1.0",
            OrganizationId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

public class CreateProgramCommandHandlerTests
{
    private readonly Mock<IRepository<Program>> _programRepositoryMock;
    private readonly Mock<IRepository<Organization>> _organizationRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProgramCommandHandler _handler;

    public CreateProgramCommandHandlerTests()
    {
        _programRepositoryMock = new Mock<IRepository<Program>>();
        _organizationRepositoryMock = new Mock<IRepository<Organization>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateProgramCommandHandler(_programRepositoryMock.Object, _organizationRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProgram()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var command = new CreateProgramCommand
        {
            Name = "Test Program",
            Description = "Test Description",
            Code = "TP",
            Version = "1.0",
            OrganizationId = organizationId,
            CreatedBy = "test-user"
        };

        var organization = Organization.Create("Test Org", "Description", "REG-001", "test-user");
        _organizationRepositoryMock
            .Setup(x => x.GetByIdAsync(organizationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(organization);

        Program? capturedProgram = null;
        _programRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Program>(), It.IsAny<CancellationToken>()))
            .Callback<Program, CancellationToken>((prog, _) => capturedProgram = prog)
            .ReturnsAsync((Program prog, CancellationToken _) => prog);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Code, result.Code);
        Assert.Equal(command.Version, result.Version);
        Assert.Equal(command.OrganizationId, result.OrganizationId);
        Assert.Equal(ProgramStatus.Draft, result.Status);

        _programRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Program>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentOrganization_ShouldThrowException()
    {
        // Arrange
        var command = new CreateProgramCommand
        {
            Name = "Test Program",
            Code = "TP",
            Version = "1.0",
            OrganizationId = Guid.NewGuid(),
            CreatedBy = "test-user"
        };

        _organizationRepositoryMock
            .Setup(x => x.GetByIdAsync(command.OrganizationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Organization?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}