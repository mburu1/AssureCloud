using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class CorrectiveAction : BaseEntity
{
    public Guid AuditId { get; private set; }
    public Guid FindingId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? RootCause { get; private set; }
    public Guid ResponsiblePartyId { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public CorrectiveActionStatus Status { get; private set; } = CorrectiveActionStatus.Open;
    public string? ActionPlan { get; private set; }
    public string? VerificationMethod { get; private set; }
    public string? VerificationNotes { get; private set; }
    public Guid? VerifiedById { get; private set; }
    public DateTime? VerifiedAt { get; private set; }
    public int Priority { get; private set; } = 3;

    private CorrectiveAction() { }

    public CorrectiveAction(
        Guid auditId,
        Guid findingId,
        string title,
        string description,
        Guid responsiblePartyId,
        DateTime dueDate,
        string? rootCause = null)
    {
        AuditId = auditId;
        FindingId = findingId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        RootCause = rootCause;
        ResponsiblePartyId = responsiblePartyId;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string title,
        string description,
        string? rootCause = null,
        string? actionPlan = null,
        string? verificationMethod = null,
        int? priority = null,
        DateTime? dueDate = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        RootCause = rootCause;
        ActionPlan = actionPlan;
        VerificationMethod = verificationMethod;
        if (priority.HasValue) Priority = priority.Value;
        if (dueDate.HasValue) DueDate = dueDate.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(CorrectiveActionStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete(string? verificationNotes = null)
    {
        CompletedDate = DateTime.UtcNow;
        VerificationNotes = verificationNotes;
        Status = CorrectiveActionStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CorrectiveActionCompletedEvent(AuditId, Id));
    }

    public void Verify(Guid userId, string? verificationNotes = null)
    {
        VerifiedById = userId;
        VerifiedAt = DateTime.UtcNow;
        VerificationNotes = verificationNotes;
        Status = CorrectiveActionStatus.Verified;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CorrectiveActionVerifiedEvent(AuditId, Id));
    }

    public void Reject(Guid userId, string? verificationNotes = null)
    {
        VerifiedById = userId;
        VerifiedAt = DateTime.UtcNow;
        VerificationNotes = verificationNotes;
        Status = CorrectiveActionStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ExtendDueDate(DateTime newDueDate, string? reason = null)
    {
        DueDate = newDueDate;
        if (reason != null)
        {
            Description += $"\n\nDue date extended: {reason}";
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsOverdue => Status != CorrectiveActionStatus.Completed &&
                              Status != CorrectiveActionStatus.Verified &&
                              DateTime.UtcNow > DueDate;
}

public class CorrectiveActionStatus : Enumeration
{
    public static readonly CorrectiveActionStatus Open = new(0, "Open");
    public static readonly CorrectiveActionStatus InProgress = new(1, "InProgress");
    public static readonly CorrectiveActionStatus Completed = new(2, "Completed");
    public static readonly CorrectiveActionStatus Verified = new(3, "Verified");
    public static readonly CorrectiveActionStatus Rejected = new(4, "Rejected");
    public static readonly CorrectiveActionStatus Overdue = new(5, "Overdue");
    public static readonly CorrectiveActionStatus Cancelled = new(6, "Cancelled");

    private CorrectiveActionStatus(int value, string name) : base(value, name) { }
}