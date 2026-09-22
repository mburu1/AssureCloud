using FluentValidation;
using AssureCloud.Application.Commands.Assessments;

namespace AssureCloud.Application.Validators.Assessments;

public class CreateAssessmentCommandValidator : AbstractValidator<CreateAssessmentCommand>
{
    public CreateAssessmentCommandValidator()
    {
        RuleFor(x => x.ProgramId)
            .NotEmpty().WithMessage("Program ID is required");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("Organization ID is required");

        RuleFor(x => x.StandardId)
            .NotEmpty().WithMessage("Standard ID is required");

        RuleFor(x => x.ScheduledDate)
            .NotEmpty().WithMessage("Scheduled date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
    }
}

public class UpdateAssessmentCommandValidator : AbstractValidator<UpdateAssessmentCommand>
{
    public UpdateAssessmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Assessment ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.AssessorNotes)
            .MaximumLength(2000).WithMessage("Assessor notes must not exceed 2000 characters");
    }
}

public class AddAssessmentResponseCommandValidator : AbstractValidator<AddAssessmentResponseCommand>
{
    public AddAssessmentResponseCommandValidator()
    {
        RuleFor(x => x.AssessmentId)
            .NotEmpty().WithMessage("Assessment ID is required");

        RuleFor(x => x.CriterionId)
            .NotEmpty().WithMessage("Criterion ID is required");

        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score must be non-negative");

        RuleFor(x => x.Comments)
            .MaximumLength(2000).WithMessage("Comments must not exceed 2000 characters");
    }
}

public class AddEvidenceCommandValidator : AbstractValidator<AddEvidenceCommand>
{
    public AddEvidenceCommandValidator()
    {
        RuleFor(x => x.AssessmentId)
            .NotEmpty().WithMessage("Assessment ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("File URL is required")
            .MaximumLength(500).WithMessage("File URL must not exceed 500 characters")
            .Must(BeValidUrl).WithMessage("File URL must be a valid URL");

        RuleFor(x => x.MimeType)
            .MaximumLength(100).WithMessage("MIME type must not exceed 100 characters");
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class CreateFindingCommandValidator : AbstractValidator<CreateFindingCommand>
{
    public CreateFindingCommandValidator()
    {
        RuleFor(x => x.AssessmentId)
            .NotEmpty().WithMessage("Assessment ID is required");

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

public class AssignAssessorCommandValidator : AbstractValidator<AssignAssessorCommand>
{
    public AssignAssessorCommandValidator()
    {
        RuleFor(x => x.AssessmentId)
            .NotEmpty().WithMessage("Assessment ID is required");

        RuleFor(x => x.AssessorId)
            .NotEmpty().WithMessage("Assessor ID is required");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(BeValidRole).WithMessage("Role must be one of: LeadAssessor, Assessor, Observer, TechnicalExpert");
    }

    private static bool BeValidRole(string role)
    {
        return role is "LeadAssessor" or "Assessor" or "Observer" or "TechnicalExpert";
    }
}