using FluentValidation;
using AssureCloud.Application.Commands.Programs;

namespace AssureCloud.Application.Validators.Programs;

public class CreateProgramCommandValidator : AbstractValidator<CreateProgramCommand>
{
    public CreateProgramCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Program name is required")
            .MaximumLength(200).WithMessage("Program name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.Version)
            .MaximumLength(20).WithMessage("Version must not exceed 20 characters");
    }
}

public class UpdateProgramCommandValidator : AbstractValidator<UpdateProgramCommand>
{
    public UpdateProgramCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Program ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Program name is required")
            .MaximumLength(200).WithMessage("Program name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500).WithMessage("Logo URL must not exceed 500 characters")
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.LogoUrl))
            .WithMessage("Logo URL must be a valid URL");
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class CreateStandardCommandValidator : AbstractValidator<CreateStandardCommand>
{
    public CreateStandardCommandValidator()
    {
        RuleFor(x => x.ProgramId)
            .NotEmpty().WithMessage("Program ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Standard name is required")
            .MaximumLength(200).WithMessage("Standard name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");
    }
}

public class CreateRequirementCommandValidator : AbstractValidator<CreateRequirementCommand>
{
    public CreateRequirementCommandValidator()
    {
        RuleFor(x => x.StandardId)
            .NotEmpty().WithMessage("Standard ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Requirement name is required")
            .MaximumLength(200).WithMessage("Requirement name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(BeValidType).WithMessage("Type must be one of: Standard, Legal, Customer, Internal, Optional");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0");

        RuleFor(x => x.Guidance)
            .MaximumLength(2000).WithMessage("Guidance must not exceed 2000 characters");

        RuleFor(x => x.ReferenceUrl)
            .MaximumLength(500).WithMessage("Reference URL must not exceed 500 characters")
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ReferenceUrl))
            .WithMessage("Reference URL must be a valid URL");
    }

    private static bool BeValidType(string type)
    {
        return type is "Standard" or "Legal" or "Customer" or "Internal" or "Optional";
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class CreateCriterionCommandValidator : AbstractValidator<CreateCriterionCommand>
{
    public CreateCriterionCommandValidator()
    {
        RuleFor(x => x.RequirementId)
            .NotEmpty().WithMessage("Requirement ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Criterion name is required")
            .MaximumLength(200).WithMessage("Criterion name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");

        RuleFor(x => x.ScoringMethod)
            .NotEmpty().WithMessage("Scoring method is required")
            .Must(BeValidScoringMethod).WithMessage("Scoring method must be one of: Percentage, Points, PassFail, Rating, Weighted");

        RuleFor(x => x.MaxScore)
            .GreaterThan(0).WithMessage("Max score must be greater than 0");

        RuleFor(x => x.PassThreshold)
            .GreaterThanOrEqualTo(0).WithMessage("Pass threshold must be non-negative")
            .LessThanOrEqualTo(x => x.MaxScore).WithMessage("Pass threshold cannot exceed max score");

        RuleFor(x => x.Guidance)
            .MaximumLength(2000).WithMessage("Guidance must not exceed 2000 characters");

        RuleFor(x => x.ReferenceUrl)
            .MaximumLength(500).WithMessage("Reference URL must not exceed 500 characters")
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ReferenceUrl))
            .WithMessage("Reference URL must be a valid URL");
    }

    private static bool BeValidScoringMethod(string method)
    {
        return method is "Percentage" or "Points" or "PassFail" or "Rating" or "Weighted";
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class CreateControlCommandValidator : AbstractValidator<CreateControlCommand>
{
    public CreateControlCommandValidator()
    {
        RuleFor(x => x.CriterionId)
            .NotEmpty().WithMessage("Criterion ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Control name is required")
            .MaximumLength(200).WithMessage("Control name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(BeValidType).WithMessage("Type must be one of: Preventive, Detective, Corrective, Compensating, Directive");

        RuleFor(x => x.Frequency)
            .GreaterThan(0).WithMessage("Frequency must be greater than 0");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0");
    }

    private static bool BeValidType(string type)
    {
        return type is "Preventive" or "Detective" or "Corrective" or "Compensating" or "Directive";
    }
}