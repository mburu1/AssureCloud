using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Criterion : BaseEntity
{
    private readonly List<Control> _controls = new();

    public Guid RequirementId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public int Order { get; private set; }
    public ScoringMethod ScoringMethod { get; private set; }
    public decimal MaxScore { get; private set; } = 100;
    public decimal PassThreshold { get; private set; } = 70;
    public bool IsCritical { get; private set; }
    public string? Guidance { get; private set; }
    public string? ReferenceUrl { get; private set; }

    public IReadOnlyCollection<Control> Controls => _controls.AsReadOnly();

    private Criterion() { }

    public Criterion(
        Guid requirementId,
        string name,
        string? description = null,
        string? code = null,
        int order = 0,
        ScoringMethod scoringMethod = ScoringMethod.Percentage)
    {
        RequirementId = requirementId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        ScoringMethod = scoringMethod;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string? description = null,
        string? code = null,
        int order = 0,
        ScoringMethod? scoringMethod = null,
        decimal? maxScore = null,
        decimal? passThreshold = null,
        bool? isCritical = null,
        string? guidance = null,
        string? referenceUrl = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Order = order;
        if (scoringMethod.HasValue) ScoringMethod = scoringMethod.Value;
        if (maxScore.HasValue) MaxScore = maxScore.Value;
        if (passThreshold.HasValue) PassThreshold = passThreshold.Value;
        if (isCritical.HasValue) IsCritical = isCritical.Value;
        Guidance = guidance;
        ReferenceUrl = referenceUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOrder(int order)
    {
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public Control AddControl(string name, string? description, string? code, ControlType type)
    {
        var control = new Control(Id, name, description, code, type);
        _controls.Add(control);
        AddDomainEvent(new ControlAddedEvent(Id, control.Id));
        return control;
    }

    public void RemoveControl(Guid controlId)
    {
        var control = _controls.FirstOrDefault(c => c.Id == controlId);
        if (control != null)
        {
            _controls.Remove(control);
            AddDomainEvent(new ControlRemovedEvent(Id, controlId));
        }
    }
}

public class ScoringMethod : Enumeration
{
    public static readonly ScoringMethod Percentage = new(0, "Percentage");
    public static readonly ScoringMethod Points = new(1, "Points");
    public static readonly ScoringMethod PassFail = new(2, "PassFail");
    public static readonly ScoringMethod Rating = new(3, "Rating");
    public static readonly ScoringMethod Weighted = new(4, "Weighted");

    private ScoringMethod(int value, string name) : base(value, name) { }
}

public class ControlType : Enumeration
{
    public static readonly ControlType Preventive = new(0, "Preventive");
    public static readonly ControlType Detective = new(1, "Detective");
    public static readonly ControlType Corrective = new(2, "Corrective");
    public static readonly ControlType Compensating = new(3, "Compensating");
    public static readonly ControlType Directive = new(4, "Directive");

    private ControlType(int value, string name) : base(value, name) { }
}