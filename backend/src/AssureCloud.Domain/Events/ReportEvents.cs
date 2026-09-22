using System;

namespace AssureCloud.Domain.Events;

public sealed class ReportCreatedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid ReportId { get; }
    public string Title { get; }

    public ReportCreatedEvent(Guid organizationId, Guid reportId, string title)
    {
        OrganizationId = organizationId;
        ReportId = reportId;
        Title = title;
    }
}

public sealed class ReportGeneratedEvent : DomainEvent
{
    public Guid ReportId { get; }
    public string FileUrl { get; }

    public ReportGeneratedEvent(Guid reportId, string fileUrl)
    {
        ReportId = reportId;
        FileUrl = fileUrl;
    }
}

public sealed class ReportGenerationFailedEvent : DomainEvent
{
    public Guid ReportId { get; }
    public string Error { get; }

    public ReportGenerationFailedEvent(Guid reportId, string error)
    {
        ReportId = reportId;
        Error = error;
    }
}

public sealed class ReportArchivedEvent : DomainEvent
{
    public Guid ReportId { get; }

    public ReportArchivedEvent(Guid reportId)
    {
        ReportId = reportId;
    }
}