using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Control : BaseEntity
{
    public Guid CriterionId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Code { get; private set; }
    public ControlType Type { get; private set; }
    public ControlStatus Status { get; private set; } = ControlStatus.Draft;
    public int Frequency { get; private set; } = 1;
    public string? FrequencyUnit { get; private set; }
    public string? ResponsibleRole { get; private set; }
    public string? EvidenceRequirements { get; private set; }
    public string? TestProcedure { get; private set; }
    public decimal Weight { get; private set; } = 1.0m;

    private Control() { }

    public Control(
        Guid criterionId,
        string name,
        string? description = null,
        string? code = null,
        ControlType type = ControlType.Preventive)
    {
        CriterionId = criterionId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        Type = type;
        FrequencyUnit = "Month";
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string? description = null,
        string? code = null,
        ControlType? type = null,
        ControlStatus? status = null,
        int? frequency = null,
        string? frequencyUnit = null,
        string? responsibleRole = null,
        string? evidenceRequirements = null,
        string? testProcedure = null,
        decimal? weight = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Code = code;
        if (type.HasValue) Type = type.Value;
        if (status.HasValue) Status = status.Value;
        if (frequency.HasValue) Frequency = frequency.Value;
        if (frequencyUnit != null) FrequencyUnit = frequencyUnit;
        ResponsibleRole = responsibleRole;
        EvidenceRequirements = evidenceRequirements;
        TestProcedure = testProcedure;
        if (weight.HasValue) Weight = weight.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(ControlStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class ControlStatus : Enumeration
{
    public static readonly ControlStatus Draft = new(0, "Draft");
    public static readonly ControlStatus Active = new(1, "Active");
    public static readonly ControlStatus Inactive = new(2, "Inactive");
    public static readonly ControlStatus UnderReview = new(3, "UnderReview");
    public static readonly ControlStatus Deprecated = new(4, "Deprecated");

    private ControlStatus(int value, string name) : base(value, name) { }
}