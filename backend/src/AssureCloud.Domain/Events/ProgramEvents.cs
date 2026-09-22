using System;

namespace AssureCloud.Domain.Events;

public sealed class ProgramCreatedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public string Name { get; }

    public ProgramCreatedEvent(Guid programId, string name)
    {
        ProgramId = programId;
        Name = name;
    }
}

public sealed class ProgramUpdatedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public string Name { get; }

    public ProgramUpdatedEvent(Guid programId, string name)
    {
        ProgramId = programId;
        Name = name;
    }
}

public sealed class ProgramDeletedEvent : DomainEvent
{
    public Guid ProgramId { get; }

    public ProgramDeletedEvent(Guid programId)
    {
        ProgramId = programId;
    }
}

public sealed class ProgramStatusChangedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public ProgramStatusChangedEvent(Guid programId, string oldStatus, string newStatus)
    {
        ProgramId = programId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class StandardAddedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public Guid StandardId { get; }

    public StandardAddedEvent(Guid programId, Guid standardId)
    {
        ProgramId = programId;
        StandardId = standardId;
    }
}

public sealed class StandardRemovedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public Guid StandardId { get; }

    public StandardRemovedEvent(Guid programId, Guid standardId)
    {
        ProgramId = programId;
        StandardId = standardId;
    }
}

public sealed class StandardUpdatedEvent : DomainEvent
{
    public Guid StandardId { get; }
    public string Name { get; }

    public StandardUpdatedEvent(Guid standardId, string name)
    {
        StandardId = standardId;
        Name = name;
    }
}

public sealed class StandardStatusChangedEvent : DomainEvent
{
    public Guid StandardId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public StandardStatusChangedEvent(Guid standardId, string oldStatus, string newStatus)
    {
        StandardId = standardId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class RequirementAddedEvent : DomainEvent
{
    public Guid StandardId { get; }
    public Guid RequirementId { get; }

    public RequirementAddedEvent(Guid standardId, Guid requirementId)
    {
        StandardId = standardId;
        RequirementId = requirementId;
    }
}

public sealed class RequirementRemovedEvent : DomainEvent
{
    public Guid StandardId { get; }
    public Guid RequirementId { get; }

    public RequirementRemovedEvent(Guid standardId, Guid requirementId)
    {
        StandardId = standardId;
        RequirementId = requirementId;
    }
}

public sealed class CriterionAddedEvent : DomainEvent
{
    public Guid RequirementId { get; }
    public Guid CriterionId { get; }

    public CriterionAddedEvent(Guid requirementId, Guid criterionId)
    {
        RequirementId = requirementId;
        CriterionId = criterionId;
    }
}

public sealed class CriterionRemovedEvent : DomainEvent
{
    public Guid RequirementId { get; }
    public Guid CriterionId { get; }

    public CriterionRemovedEvent(Guid requirementId, Guid criterionId)
    {
        RequirementId = requirementId;
        CriterionId = criterionId;
    }
}

public sealed class ControlAddedEvent : DomainEvent
{
    public Guid CriterionId { get; }
    public Guid ControlId { get; }

    public ControlAddedEvent(Guid criterionId, Guid controlId)
    {
        CriterionId = criterionId;
        ControlId = controlId;
    }
}

public sealed class ControlRemovedEvent : DomainEvent
{
    public Guid CriterionId { get; }
    public Guid ControlId { get; }

    public ControlRemovedEvent(Guid criterionId, Guid controlId)
    {
        CriterionId = criterionId;
        ControlId = controlId;
    }
}