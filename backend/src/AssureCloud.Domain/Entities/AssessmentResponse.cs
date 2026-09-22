using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class AssessmentResponse : BaseEntity
{
    public Guid AssessmentId { get; private set; }
    public Guid CriterionId { get; private set; }
    public decimal Score { get; private set; }
    public string? Comments { get; private set; }
    public bool IsManual { get; private set; }
    public DateTime? RespondedAt { get; private set; }
    public Guid? RespondedById { get; private set; }

    private AssessmentResponse() { }

    public AssessmentResponse(
        Guid assessmentId,
        Guid criterionId,
        decimal score,
        string? comments = null,
        bool isManual = false)
    {
        AssessmentId = assessmentId;
        CriterionId = criterionId;
        Score = score;
        Comments = comments;
        IsManual = isManual;
        RespondedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateResponse(decimal score, string? comments = null, bool? isManual = null)
    {
        Score = score;
        if (comments != null) Comments = comments;
        if (isManual.HasValue) IsManual = isManual.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsResponded(Guid userId)
    {
        RespondedAt = DateTime.UtcNow;
        RespondedById = userId;
        UpdatedAt = DateTime.UtcNow;
    }
}