using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Report : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid? ProgramId { get; private set; }
    public Guid? AssessmentId { get; private set; }
    public Guid? AuditId { get; private set; }
    public Guid? CertificationId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ReportType Type { get; private set; }
    public ReportStatus Status { get; private set; } = ReportStatus.Draft;
    public string? FileUrl { get; private set; }
    public string? MimeType { get; private set; }
    public long? FileSize { get; private set; }
    public Guid? GeneratedById { get; private set; }
    public DateTime? GeneratedAt { get; private set; }
    public DateTime? PeriodStart { get; private set; }
    public DateTime? PeriodEnd { get; private set; }
    public string? Parameters { get; private set; }
    public int Version { get; private set; } = 1;

    private Report() { }

    public Report(
        Guid organizationId,
        string title,
        ReportType type,
        Guid? programId = null,
        Guid? assessmentId = null,
        Guid? auditId = null,
        Guid? certificationId = null,
        string? description = null)
    {
        OrganizationId = organizationId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Type = type;
        ProgramId = programId;
        AssessmentId = assessmentId;
        AuditId = auditId;
        CertificationId = certificationId;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string? description, string? parameters)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Parameters = parameters;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(ReportStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetGenerated(Guid userId, string fileUrl, string? mimeType = null, long? fileSize = null)
    {
        GeneratedById = userId;
        GeneratedAt = DateTime.UtcNow;
        FileUrl = fileUrl;
        MimeType = mimeType;
        FileSize = fileSize;
        Status = ReportStatus.Generated;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ReportGeneratedEvent(Id, fileUrl));
    }

    public void SetPeriod(DateTime periodStart, DateTime periodEnd)
    {
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        UpdatedAt = DateTime.UtcNow;
    }

    public Report CreateNewVersion(string title, string? description)
    {
        var newReport = new Report(OrganizationId, title, Type, ProgramId, AssessmentId, AuditId, CertificationId, description)
        {
            Version = Version + 1
        };
        return newReport;
    }
}

public class ReportType : Enumeration
{
    public static readonly ReportType Assessment = new(0, "Assessment");
    public static readonly ReportType Audit = new(1, "Audit");
    public static readonly ReportType Certification = new(2, "Certification");
    public static readonly ReportType Compliance = new(3, "Compliance");
    public static readonly ReportType Surveillance = new(4, "Surveillance");
    public static readonly ReportType Management = new(5, "Management");
    public static readonly ReportType ExecutiveSummary = new(6, "ExecutiveSummary");
    public static readonly ReportType Custom = new(7, "Custom");

    private ReportType(int value, string name) : base(value, name) { }
}

public class ReportStatus : Enumeration
{
    public static readonly ReportStatus Draft = new(0, "Draft");
    public static readonly ReportStatus Generating = new(1, "Generating");
    public static readonly ReportStatus Generated = new(2, "Generated");
    public static readonly ReportStatus Failed = new(3, "Failed");
    public static readonly ReportStatus Archived = new(4, "Archived");

    private ReportStatus(int value, string name) : base(value, name) { }
}