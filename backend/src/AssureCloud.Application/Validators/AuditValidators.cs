using FluentValidation;
using AssureCloud.Application.Commands.Audits;

namespace AssureCloud.Application.Validators.Audits;

public class CreateAuditCommandValidator : AbstractValidator<CreateAuditCommand>
{
    public CreateAuditCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("Organization ID is required");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Audit type is required")
            .Must(BeValidType).WithMessage("Type must be one of: Internal, External, Supplier, Surveillance, Recertification, Special, FollowUp");

        RuleFor(x => x.PlannedStartDate)
            .NotEmpty().WithMessage("Planned start date is required")
            .LessThan(x => x.PlannedEndDate).WithMessage("Planned start date must be before planned end date");

        RuleFor(x => x.PlannedEndDate)
            .NotEmpty().WithMessage("Planned end date is required")
            .GreaterThan(x => x.PlannedStartDate).WithMessage("Planned end date must be after planned start date");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Scope)
            .MaximumLength(2000).WithMessage("Scope must not exceed 2000 characters");

        RuleFor(x => x.Criteria)
            .MaximumLength(2000).WithMessage("Criteria must not exceed 2000 characters");
    }

    private static bool BeValidType(string type)
    {
        return type is "Internal" or "External" or "Supplier" or "Surveillance" or "Recertification" or "Special" or "FollowUp";
    }
}

public class UpdateAuditCommandValidator : AbstractValidator<UpdateAuditCommand>
{
    public UpdateAuditCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Audit ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Type)
            .Must(BeValidType).When(x => !string.IsNullOrEmpty(x.Type))
            .WithMessage("Type must be one of: Internal, External, Supplier, Surveillance, Recertification, Special, FollowUp");

        RuleFor(x => x.PlannedStartDate)
            .LessThan(x => x.PlannedEndDate).When(x => x.PlannedStartDate.HasValue && x.PlannedEndDate.HasValue)
            .WithMessage("Planned start date must be before planned end date");

        RuleFor(x => x.Scope)
            .MaximumLength(2000).WithMessage("Scope must not exceed 2000 characters");

        RuleFor(x => x.Criteria)
            .MaximumLength(2000).WithMessage("Criteria must not exceed 2000 characters");
    }

    private static bool BeValidType(string type)
    {
        return type is "Internal" or "External" or "Supplier" or "Surveillance" or "Recertification" or "Special" or "FollowUp";
    }
}

public class CreateAuditFindingCommandValidator : AbstractValidator<CreateAuditFindingCommand>
{
    public CreateAuditFindingCommandValidator()
    {
        RuleFor(x => x.AuditId)
            .NotEmpty().WithMessage("Audit ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Severity)
            .NotEmpty().WithMessage("Severity is required")
            .Must(BeValidSeverity).WithMessage("Severity must be one of: Low, Medium, High, Critical, Informational");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .Must(BeValidCategory).WithMessage("Category must be one of: NonConformity, Observation, OpportunityForImprovement, PositiveFinding, MinorNonConformity, MajorNonConformity");

        RuleFor(x => x.Requirement)
            .MaximumLength(500).WithMessage("Requirement must not exceed 500 characters");

        RuleFor(x => x.RootCause)
            .MaximumLength(2000).WithMessage("Root cause must not exceed 2000 characters");

        RuleFor(x => x.Impact)
            .MaximumLength(2000).WithMessage("Impact must not exceed 2000 characters");

        RuleFor(x => x.Recommendation)
            .MaximumLength(2000).WithMessage("Recommendation must not exceed 2000 characters");
    }

    private static bool BeValidSeverity(string severity)
    {
        return severity is "Low" or "Medium" or "High" or "Critical" or "Informational";
    }

    private static bool BeValidCategory(string category)
    {
        return category is "NonConformity" or "Observation" or "OpportunityForImprovement" or "PositiveFinding" or "MinorNonConformity" or "MajorNonConformity";
    }
}

public class CreateCorrectiveActionCommandValidator : AbstractValidator<CreateCorrectiveActionCommand>
{
    public CreateCorrectiveActionCommandValidator()
    {
        RuleFor(x => x.AuditId)
            .NotEmpty().WithMessage("Audit ID is required");

        RuleFor(x => x.FindingId)
            .NotEmpty().WithMessage("Finding ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.ResponsiblePartyId)
            .NotEmpty().WithMessage("Responsible party ID is required");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future");

        RuleFor(x => x.RootCause)
            .MaximumLength(2000).WithMessage("Root cause must not exceed 2000 characters");

        RuleFor(x => x.ActionPlan)
            .MaximumLength(2000).WithMessage("Action plan must not exceed 2000 characters");

        RuleFor(x => x.VerificationMethod)
            .MaximumLength(1000).WithMessage("Verification method must not exceed 1000 characters");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5");
    }
}

public class AssignAuditorCommandValidator : AbstractValidator<AssignAuditorCommand>
{
    public AssignAuditorCommandValidator()
    {
        RuleFor(x => x.AuditId)
            .NotEmpty().WithMessage("Audit ID is required");

        RuleFor(x => x.AuditorId)
            .NotEmpty().WithMessage("Auditor ID is required");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(BeValidRole).WithMessage("Role must be one of: LeadAuditor, Auditor, TechnicalExpert, Observer, Trainee");
    }

    private static bool BeValidRole(string role)
    {
        return role is "LeadAuditor" or "Auditor" or "TechnicalExpert" or "Observer" or "Trainee";
    }
}