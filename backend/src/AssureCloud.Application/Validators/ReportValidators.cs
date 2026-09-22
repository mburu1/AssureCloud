using FluentValidation;
using AssureCloud.Application.Commands.Reports;

namespace AssureCloud.Application.Validators.Reports;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("Organization ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Report type is required")
            .Must(BeValidType).WithMessage("Type must be one of: Assessment, Audit, Certification, Compliance, Surveillance, Management, ExecutiveSummary, Custom");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Parameters)
            .MaximumLength(5000).WithMessage("Parameters must not exceed 5000 characters");
    }

    private static bool BeValidType(string type)
    {
        return type is "Assessment" or "Audit" or "Certification" or "Compliance" or "Surveillance" or "Management" or "ExecutiveSummary" or "Custom";
    }
}

public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
{
    public UpdateReportCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Report ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Parameters)
            .MaximumLength(5000).WithMessage("Parameters must not exceed 5000 characters");
    }
}

public class GenerateReportCommandValidator : AbstractValidator<GenerateReportCommand>
{
    public GenerateReportCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Report ID is required");

        RuleFor(x => x.Parameters)
            .MaximumLength(5000).WithMessage("Parameters must not exceed 5000 characters");
    }
}