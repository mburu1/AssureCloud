using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Certifications;

public record CreateCertificationCommand(
    Guid OrganizationId,
    Guid ProgramId,
    Guid? AssessmentId = null,
    string? Title = null,
    string? CertificateNumber = null,
    int SurveillanceIntervalMonths = 12) : ICommand<CertificationDto>;

public record UpdateCertificationCommand(
    Guid Id,
    string? Title = null,
    string? CertificateNumber = null,
    int? SurveillanceIntervalMonths = null) : ICommand<CertificationDto>;

public record DeleteCertificationCommand(Guid Id) : ICommand;

public record UpdateCertificationStatusCommand(Guid Id, string Status) : ICommand;

public record IssueCertificationCommand(Guid Id, Guid IssuedById, string? CertificateUrl = null) : ICommand;

public record SuspendCertificationCommand(Guid Id, string Reason) : ICommand;

public record RevokeCertificationCommand(Guid Id, string Reason) : ICommand;

public record RenewCertificationCommand(Guid Id, Guid RenewedById, DateTime NewExpirationDate) : ICommand;

public record SetCertificationCertificateNumberCommand(Guid Id, string CertificateNumber) : ICommand;

public record AddCertificationDecisionCommand(
    Guid CertificationId,
    string Decision,
    string Rationale,
    Guid DecidedById,
    string Type) : ICommand<CertificationDecisionDto>;

public record AddCertificationScopeCommand(
    Guid CertificationId,
    string Name,
    string? Description,
    Guid StandardId) : ICommand<CertificationScopeDto>;

public record UpdateCertificationScopeCommand(
    Guid Id,
    string Name,
    string? Description) : ICommand<CertificationScopeDto>;

public record DeleteCertificationScopeCommand(Guid CertificationId, Guid ScopeId) : ICommand;

public record UpdateSurveillanceDueCommand(Guid Id, DateTime DueDate) : ICommand;