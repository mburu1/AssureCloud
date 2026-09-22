using System;
using AssureCloud.Domain.Entities;
using Xunit;

namespace AssureCloud.Domain.Tests.Entities;

public class AssessmentTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAssessment()
    {
        // Arrange
        var title = "Annual Security Assessment";
        var programId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var createdBy = "test-user";

        // Act
        var assessment = Assessment.Create(title, programId, organizationId, createdBy);

        // Assert
        Assert.NotNull(assessment);
        Assert.Equal(title, assessment.Title);
        Assert.Equal(programId, assessment.ProgramId);
        Assert.Equal(organizationId, assessment.OrganizationId);
        Assert.Equal(AssessmentStatus.Draft, assessment.Status);
        Assert.Equal("1.0", assessment.Version);
        Assert.NotEqual(Guid.Empty, assessment.Id);
        Assert.Equal(createdBy, assessment.CreatedBy);
    }

    [Fact]
    public void Start_ShouldChangeStatusToInProgressAndRaiseEvent()
    {
        // Arrange
        var assessment = Assessment.Create("Test Assessment", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        assessment.ClearDomainEvents();

        // Act
        assessment.Start("assessor-user");

        // Assert
        Assert.Equal(AssessmentStatus.InProgress, assessment.Status);
        Assert.NotNull(assessment.StartedAt);
        Assert.Equal("assessor-user", assessment.UpdatedBy);
        Assert.Single(assessment.DomainEvents);
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompletedAndRaiseEvent()
    {
        // Arrange
        var assessment = Assessment.Create("Test Assessment", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        assessment.Start("assessor-user");
        assessment.ClearDomainEvents();

        // Act
        assessment.Complete(85.5m, 100m, "assessor-user");

        // Assert
        Assert.Equal(AssessmentStatus.Completed, assessment.Status);
        Assert.Equal(85.5m, assessment.OverallScore);
        Assert.Equal(100m, assessment.MaxPossibleScore);
        Assert.NotNull(assessment.CompletedAt);
        Assert.Equal("assessor-user", assessment.UpdatedBy);
        Assert.Single(assessment.DomainEvents);
    }

    [Fact]
    public void SubmitResponse_ShouldAddResponseAndRaiseEvent()
    {
        // Arrange
        var assessment = Assessment.Create("Test Assessment", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        var criterionId = Guid.NewGuid();
        var respondentId = Guid.NewGuid();
        assessment.ClearDomainEvents();

        // Act
        var response = assessment.SubmitResponse(criterionId, respondentId, "Compliant", "All controls implemented", 10m, 10m, "respondent-user");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(criterionId, response.CriterionId);
        Assert.Equal(respondentId, response.RespondentId);
        Assert.Equal("Compliant", response.ResponseValue);
        Assert.Equal(AssessmentResponseStatus.Submitted, response.Status);
        Assert.Single(assessment.Responses);
        Assert.Single(assessment.DomainEvents);
    }

    [Fact]
    public void UploadEvidence_ShouldAddEvidenceAndRaiseEvent()
    {
        // Arrange
        var assessment = Assessment.Create("Test Assessment", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        var criterionId = Guid.NewGuid();
        var uploadedById = Guid.NewGuid();
        assessment.ClearDomainEvents();

        // Act
        var evidence = assessment.UploadEvidence(
            "Policy Document",
            "Information Security Policy",
            "policy.pdf",
            "/uploads/policy.pdf",
            "application/pdf",
            102400,
            "sha256-hash",
            criterionId,
            uploadedById,
            "uploader-user");

        // Assert
        Assert.NotNull(evidence);
        Assert.Equal("Policy Document", evidence.Title);
        Assert.Equal("policy.pdf", evidence.FileName);
        Assert.Equal(EvidenceStatus.Uploaded, evidence.Status);
        Assert.Single(assessment.Evidence);
        Assert.Single(assessment.DomainEvents);
    }

    [Fact]
    public void CreateFinding_ShouldAddFindingAndRaiseEvent()
    {
        // Arrange
        var assessment = Assessment.Create("Test Assessment", Guid.NewGuid(), Guid.NewGuid(), "test-user");
        var criterionId = Guid.NewGuid();
        assessment.ClearDomainEvents();

        // Act
        var finding = assessment.CreateFinding(
            "Missing Encryption Policy",
            "No encryption policy found for data at rest",
            "Implement encryption policy per ISO 27001 A.10.1",
            FindingSeverity.High,
            FindingCategory.NonConformity,
            criterionId,
            null,
            "auditor-user");

        // Assert
        Assert.NotNull(finding);
        Assert.Equal("Missing Encryption Policy", finding.Title);
        Assert.Equal(FindingSeverity.High, finding.Severity);
        Assert.Equal(FindingCategory.NonConformity, finding.Category);
        Assert.Equal(FindingStatus.Open, finding.Status);
        Assert.Single(assessment.Findings);
        Assert.Single(assessment.DomainEvents);
    }
}