using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Standard : BaseEntity
{
    private readonly List<Requirement> _requirements = new();

    public Guid ProgramId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public int Order { get; private set; }
    public StandardStatus Status { get; private set; } = StandardStatus.Draft;
    public string? Version { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    public IReadOnlyCollection<Requirement> Requirements => _requirements.AsReadOnly();

    private Standard() { }

    public Standard(
        Guid programId,
        string name,
        string? description = null,
        string? code = null,
        int order = 0)
    {
        ProgramId = programId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        Version = "1.0.0";
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description, string? code, int order)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(StandardStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetVersion(string version)
    {
        Version = version ?? throw new ArgumentNullException(nameof(version));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDates(DateTime? effectiveDate, DateTime? expirationDate)
    {
        EffectiveDate = effectiveDate;
        ExpirationDate = expirationDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public Requirement AddRequirement(string name, string? description, string? code, int order, RequirementType type)
    {
        var requirement = new Requirement(Id, name, description, code, order, type);
        _requirements.Add(requirement);
        AddDomainEvent(new RequirementAddedEvent(Id, requirement.Id));
        return requirement;
    }

    public void RemoveRequirement(Guid requirementId)
    {
        var requirement = _requirements.FirstOrDefault(r => r.Id == requirementId);
        if (requirement != null)
        {
            _requirements.Remove(requirement);
            AddDomainEvent(new RequirementRemovedEvent(Id, requirementId));
        }
    }

    public void ReorderRequirements(IEnumerable<Guid> requirementIdsInOrder)
    {
        var orderedIds = requirementIdsInOrder.ToList();
        for (int i = 0; i < orderedIds.Count; i++)
        {
            var req = _requirements.FirstOrDefault(r => r.Id == orderedIds[i]);
            if (req != null)
            {
                req.SetOrder(i);
            }
        }
        UpdatedAt = DateTime.UtcNow;
    }
}

public class StandardStatus : Enumeration
{
    public static readonly StandardStatus Draft = new(0, "Draft");
    public static readonly StandardStatus Active = new(1, "Active");
    public static readonly StandardStatus Inactive = new(2, "Inactive");
    public static readonly StandardStatus Archived = new(3, "Archived");
    public static readonly StandardStatus Deprecated = new(4, "Deprecated");

    private StandardStatus(int value, string name) : base(value, name) { }
}