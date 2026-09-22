using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Program : BaseEntity
{
    private readonly List<Standard> _standards = new();
    private readonly List<Assessment> _assessments = new();

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public ProgramStatus Status { get; private set; } = ProgramStatus.Draft;
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public string? Version { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string? LogoUrl { get; private set; }

    public IReadOnlyCollection<Standard> Standards => _standards.AsReadOnly();
    public IReadOnlyCollection<Assessment> Assessments => _assessments.AsReadOnly();

    private Program() { }

    public Program(string name, string? description = null, string? code = null, string? version = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Version = version ?? "1.0.0";
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description, string? code, string? logoUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        LogoUrl = logoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(ProgramStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDates(DateTime? effectiveDate, DateTime? expirationDate)
    {
        EffectiveDate = effectiveDate;
        ExpirationDate = expirationDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetVersion(string version)
    {
        Version = version ?? throw new ArgumentNullException(nameof(version));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOwner(Guid ownerId)
    {
        OwnerId = ownerId;
        UpdatedAt = DateTime.UtcNow;
    }

    public Standard AddStandard(string name, string? description, string? code, int order)
    {
        var standard = new Standard(Id, name, description, code, order);
        _standards.Add(standard);
        AddDomainEvent(new StandardAddedEvent(Id, standard.Id));
        return standard;
    }

    public void RemoveStandard(Guid standardId)
    {
        var standard = _standards.FirstOrDefault(s => s.Id == standardId);
        if (standard != null)
        {
            _standards.Remove(standard);
            AddDomainEvent(new StandardRemovedEvent(Id, standardId));
        }
    }

    public Assessment CreateAssessment(Guid organizationId, Guid standardId, DateTime scheduledDate, string? notes = null)
    {
        var assessment = new Assessment(Id, organizationId, standardId, scheduledDate, notes);
        _assessments.Add(assessment);
        AddDomainEvent(new AssessmentCreatedEvent(Id, assessment.Id));
        return assessment;
    }
}

public class ProgramStatus : Enumeration
{
    public static readonly ProgramStatus Draft = new(0, "Draft");
    public static readonly ProgramStatus Active = new(1, "Active");
    public static readonly ProgramStatus Inactive = new(2, "Inactive");
    public static readonly ProgramStatus Archived = new(3, "Archived");
    public static readonly ProgramStatus PendingApproval = new(4, "PendingApproval");
    public static readonly ProgramStatus Suspended = new(5, "Suspended");

    private ProgramStatus(int value, string name) : base(value, name) { }
}