using System;
using AssureCloud.Domain.Entities;
using Xunit;

namespace AssureCloud.Domain.Tests.Entities;

public class AuditTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAudit()
    {
        // Arrange
        var title = "ISO 27001 Surveillance Audit";
        var programId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var createdBy = "test-user";

        // Act
        var audit = Audit.Create(title, programId, organizationId, createdBy);

        // Assert
        Assert.NotNull(audit);
        Assert.Equal(title, audit.Title);
        Assert.Equal(programId, audit.ProgramId);
        Assert.Equal(organizationId, audit.OrganizationId);
        Assert.Equal(AuditStatus.Planned, audit.Status);
        Assert.Equal(AuditType.Surveillance, audit.Type);
        Assert.NotEqual(Guid.Empty, audit.Id);
        Assert.Equal(createdBy, audit.CreatedBy);
    }

    [Fact]
    public void Start_ShouldChangeStatusToInProgressAndRaiseEvent()
    {
        // Arrange
        var audit = Audit.Create("Test Audit", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        audit.ClearDomainEvents();

        // Act
        audit.Start("lead-auditor-user");

        // Assert
        Assert.Equal(AuditStatus.InProgress, audit.Status);
        Assert.NotNull(audit.ActualStartDate);
        Assert.Equal("lead-auditor-user", audit.UpdatedBy);
        Assert.Single(audit.DomainEvents);
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompletedAndRaiseEvent()
    {
        // Arrange
        var audit = Audit.Create("Test Audit", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        audit.Start("lead-auditor-user");
        audit.ClearDomainEvents();

        // Act
        audit.Complete(AuditRating.Recommended, "AUD-2024-001", "lead-auditor-user");

        // Assert
        Assert.Equal(AuditStatus.Completed, audit.Status);
        Assert.Equal(AuditRating.Recommended, audit.OverallRating);
        Assert.Equal("AUD-2024-001", audit.ReportReference);
        Assert.NotNull(audit.ActualEndDate);
        Assert.Equal("lead-auditor-user", audit.UpdatedBy);
        Assert.Single(audit.DomainEvents);
    }

    [Fact]
    public void CreateFinding_ShouldAddFindingAndRaiseEvent()
    {
        // Arrange
        var audit = Audit.Create("Test Audit", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        audit.ClearDomainEvents();

        // Act
        var finding = audit.CreateFinding(
            "Weak Access Controls",
            "Access controls not properly implemented",
            "Implement MFA for all admin accounts",
            AuditFindingSeverity.Major,
            AuditFindingCategory.NonConformity,
            "ISO 27001 A.9.2",
            "auditor-user");

        // Assert
        Assert.NotNull(finding);
        Assert.Equal("Weak Access Controls", finding.Title);
        Assert.Equal(AuditFindingSeverity.Major, finding.Severity);
        Assert.Equal(AuditFindingCategory.NonConformity, finding.Category);
        Assert.Equal(AuditFindingStatus.Open, finding.Status);
        Assert.Single(audit.Findings);
        Assert.Single(audit.DomainEvents);
    }

    [Fact]
    public void CreateCorrectiveAction_ShouldAddCorrectiveActionAndRaiseEvent()
    {
        // Arrange
        var audit = Audit.Create("Test Audit", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        var finding = audit.CreateFinding(
            "Test Finding",
            "Description",
            "Recommendation",
            AuditFindingSeverity.Minor,
            AuditFindingCategory.Observation,
            "ISO 27001 A.9.2",
            "auditor-user");
        audit.ClearDomainEvents();

        // Act
        var correctiveAction = audit.CreateCorrectiveAction(
            finding.Id,
            "Implement MFA",
            "Root cause: legacy system",
            "Implement MFA for all admin accounts",
            DateTime.UtcNow.AddDays(30),
            CorrectiveActionPriority.High,
            "admin-user");

        // Assert
        Assert.NotNull(correctiveAction);
        Assert.Equal(finding.Id, correctiveAction.FindingId);
        Assert.Equal("Implement MFA", correctiveAction.Title);
        Assert.Equal(CorrectiveActionStatus.Open, correctiveAction.Status);
        Assert.Equal(CorrectiveActionPriority.High, correctiveAction.Priority);
        Assert.Single(audit.CorrectiveActions);
        Assert.Single(audit.DomainEvents);
    }
}