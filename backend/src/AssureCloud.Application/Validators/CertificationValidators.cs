using FluentValidation;
using AssureCloud.Application.Commands.Certifications;

namespace AssureCloud.Application.Validators.Certifications;

public class CreateCertificationCommandValidator : AbstractValidator<CreateCertificationCommand>
{
    public CreateCertificationCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("Organization ID is required");

        RuleFor(x => x.ProgramId)
            .NotEmpty().WithMessage("Program ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.CertificateNumber)
            .MaximumLength(50).WithMessage("Certificate number must not exceed 50 characters");

        RuleFor(x => x.SurveillanceIntervalMonths)
            .InclusiveBetween(1, 36).WithMessage("Surveillance interval must be between 1 and 36 months");
    }
}

public class UpdateCertificationCommandValidator : AbstractValidator<UpdateCertificationCommand>
{
    public UpdateCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.CertificateNumber)
            .MaximumLength(50).WithMessage("Certificate number must not exceed 50 characters");

        RuleFor(x => x.SurveillanceIntervalMonths)
            .InclusiveBetween(1, 36).When(x => x.SurveillanceIntervalMonths.HasValue)
            .WithMessage("Surveillance interval must be between 1 and 36 months");
    }
}

public class IssueCertificationCommandValidator : AbstractValidator<IssueCertificationCommand>
{
    public IssueCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.IssuedById)
            .NotEmpty().WithMessage("Issued by ID is required");

        RuleFor(x => x.CertificateUrl)
            .MaximumLength(500).WithMessage("Certificate URL must not exceed 500 characters")
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.CertificateUrl))
            .WithMessage("Certificate URL must be a valid URL");
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

public class SuspendCertificationCommandValidator : AbstractValidator<SuspendCertificationCommand>
{
    public SuspendCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Suspension reason is required")
            .MaximumLength(1000).WithMessage("Reason must not exceed 1000 characters");
    }
}

public class RevokeCertificationCommandValidator : AbstractValidator<RevokeCertificationCommand>
{
    public RevokeCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Revocation reason is required")
            .MaximumLength(1000).WithMessage("Reason must not exceed 1000 characters");
    }
}

public class RenewCertificationCommandValidator : AbstractValidator<RenewCertificationCommand>
{
    public RenewCertificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.RenewedById)
            .NotEmpty().WithMessage("Renewed by ID is required");

        RuleFor(x => x.NewExpirationDate)
            .NotEmpty().WithMessage("New expiration date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("New expiration date must be in the future");
    }
}

public class AddCertificationDecisionCommandValidator : AbstractValidator<AddCertificationDecisionCommand>
{
    public AddCertificationDecisionCommandValidator()
    {
        RuleFor(x => x.CertificationId)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.Decision)
            .NotEmpty().WithMessage("Decision is required")
            .MaximumLength(500).WithMessage("Decision must not exceed 500 characters");

        RuleFor(x => x.Rationale)
            .NotEmpty().WithMessage("Rationale is required")
            .MaximumLength(2000).WithMessage("Rationale must not exceed 2000 characters");

        RuleFor(x => x.DecidedById)
            .NotEmpty().WithMessage("Decided by ID is required");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(BeValidType).WithMessage("Type must be one of: InitialCertification, Surveillance, Recertification, Suspension, Revocation, ScopeExtension, ScopeReduction");
    }

    private static bool BeValidType(string type)
    {
        return type is "InitialCertification" or "Surveillance" or "Recertification" or "Suspension" or "Revocation" or "ScopeExtension" or "ScopeReduction";
    }
}

public class AddCertificationScopeCommandValidator : AbstractValidator<AddCertificationScopeCommand>
{
    public AddCertificationScopeCommandValidator()
    {
        RuleFor(x => x.CertificationId)
            .NotEmpty().WithMessage("Certification ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Scope name is required")
            .MaximumLength(200).WithMessage("Scope name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.StandardId)
            .NotEmpty().WithMessage("Standard ID is required");
    }
}