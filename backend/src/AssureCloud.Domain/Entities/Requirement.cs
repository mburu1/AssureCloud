using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Requirement : BaseEntity
{
    private readonly List<Criterion> _criteria = new();

    public Guid StandardId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public int Order { get; private set; }
    public RequirementType? Type { get; private set; }
    public RequirementStatus? Status { get; private set; } = RequirementStatus.Draft;
    public bool IsMandatory { get; private set; } = true;
    public decimal Weight { get; private set; } = 1.0m;
    public string? Guidance { get; private set; }
    public string? ReferenceUrl { get; private set; }

    public IReadOnlyCollection<Criterion> Criteria => _criteria.AsReadOnly();

    private Requirement() { }

    public Requirement(
        Guid standardId,
        string name,
        string? description = null,
        string? code = null,
        int order = 0,
        RequirementType? type = null)
    {
        StandardId = standardId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        Type = type ?? RequirementType.Standard;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string? description = null,
        string? code = null,
        int order = 0,
        RequirementType? type = null,
        bool? isMandatory = null,
        decimal? weight = null,
        string? guidance = null,
        string? referenceUrl = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        if (type != null) Type = type;
        if (isMandatory.HasValue) IsMandatory = isMandatory.Value;
        if (weight.HasValue) Weight = weight.Value;
        Guidance = guidance;
        ReferenceUrl = referenceUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(RequirementStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOrder(int order)
    {
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public Criterion AddCriterion(string name, string? description, string? code, int order, ScoringMethod scoringMethod)
    {
        var criterion = new Criterion(Id, name, description, code, order, scoringMethod);
        _criteria.Add(criterion);
        AddDomainEvent(new CriterionAddedEvent(Id, criterion.Id));
        return criterion;
    }

    public void RemoveCriterion(Guid criterionId)
    {
        var criterion = _criteria.FirstOrDefault(c => c.Id == criterionId);
        if (criterion != null)
        {
            _criteria.Remove(criterion);
            AddDomainEvent(new CriterionRemovedEvent(Id, criterionId));
        }
    }

    public void ReorderCriteria(IEnumerable<Guid> criterionIdsInOrder)
    {
        var orderedIds = criterionIdsInOrder.ToList();
        for (int i = 0; i < orderedIds.Count; i++)
        {
            var criterion = _criteria.FirstOrDefault(c => c.Id == orderedIds[i]);
            if (criterion != null)
            {
                criterion.SetOrder(i);
            }
        }
        UpdatedAt = DateTime.UtcNow;
    }
}

public class RequirementType : Enumeration
{
    public static readonly RequirementType Standard = new(0, "Standard");
    public static readonly RequirementType Legal = new(1, "Legal");
    public static readonly RequirementType Customer = new(2, "Customer");
    public static readonly RequirementType Internal = new(3, "Internal");
    public static readonly RequirementType Optional = new(4, "Optional");

    private RequirementType(int value, string name) : base(value, name) { }
}

public class RequirementStatus : Enumeration
{
    public static readonly RequirementStatus Draft = new(0, "Draft");
    public static readonly RequirementStatus Active = new(1, "Active");
    public static readonly RequirementStatus Inactive = new(2, "Inactive");
    public static readonly RequirementStatus Deprecated = new(3, "Deprecated");

    private RequirementStatus(int value, string name) : base(value, name) { }
}