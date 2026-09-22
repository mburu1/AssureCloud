using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Evidence : BaseEntity
{
    public Guid AssessmentId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string FileUrl { get; private set; } = string.Empty;
    public string? MimeType { get; private set; }
    public long? FileSize { get; private set; }
    public EvidenceStatus Status { get; private set; } = EvidenceStatus.Submitted;
    public Guid? SubmittedById { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public Guid? VerifiedById { get; private set; }
    public DateTime? VerifiedAt { get; private set; }
    public string? VerificationNotes { get; private set; }
    public int Version { get; private set; } = 1;
    public Guid? PreviousVersionId { get; private set; }

    private Evidence() { }

    public Evidence(
        Guid assessmentId,
        string title,
        string? description,
        string fileUrl,
        string? mimeType = null,
        long? fileSize = null)
    {
        AssessmentId = assessmentId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        FileUrl = fileUrl ?? throw new ArgumentNullException(nameof(fileUrl));
        MimeType = mimeType;
        FileSize = fileSize;
        SubmittedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string? description, string? fileUrl, string? mimeType = null, long? fileSize = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        if (fileUrl != null) FileUrl = fileUrl;
        MimeType = mimeType;
        FileSize = fileSize;
        Version++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(EvidenceStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Submit(Guid userId)
    {
        SubmittedById = userId;
        SubmittedAt = DateTime.UtcNow;
        Status = EvidenceStatus.Submitted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Verify(Guid userId, string? notes = null)
    {
        VerifiedById = userId;
        VerifiedAt = DateTime.UtcNow;
        VerificationNotes = notes;
        Status = EvidenceStatus.Verified;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject(Guid userId, string? notes = null)
    {
        VerifiedById = userId;
        VerifiedAt = DateTime.UtcNow;
        VerificationNotes = notes;
        Status = EvidenceStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public Evidence CreateNewVersion(string title, string? description, string fileUrl, string? mimeType = null, long? fileSize = null)
    {
        var newEvidence = new Evidence(AssessmentId, title, description, fileUrl, mimeType, fileSize)
        {
            Version = Version + 1,
            PreviousVersionId = Id
        };
        return newEvidence;
    }
}

public class EvidenceStatus : Enumeration
{
    public static readonly EvidenceStatus Draft = new(0, "Draft");
    public static readonly EvidenceStatus Submitted = new(1, "Submitted");
    public static readonly EvidenceStatus UnderReview = new(2, "UnderReview");
    public static readonly EvidenceStatus Verified = new(3, "Verified");
    public static readonly EvidenceStatus Rejected = new(4, "Rejected");
    public static readonly EvidenceStatus Expired = new(5, "Expired");

    private EvidenceStatus(int value, string name) : base(value, name) { }
}