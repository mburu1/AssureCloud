using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Organizations;

public record CreateOrganizationCommand(
    string Name,
    string? Description = null,
    string? RegistrationNumber = null,
    string? TaxId = null,
    string? Website = null) : ICommand<OrganizationDto>;

public record UpdateOrganizationCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Website = null,
    string? LogoUrl = null) : ICommand<OrganizationDto>;

public record DeleteOrganizationCommand(Guid Id) : ICommand;

public record UpdateOrganizationStatusCommand(Guid Id, string Status) : ICommand;

public record CreateOrganizationLocationCommand(
    Guid OrganizationId,
    string Name,
    string Address,
    string City,
    string State,
    string Country,
    string PostalCode,
    bool IsPrimary = false,
    double? Latitude = null,
    double? Longitude = null,
    string? Timezone = null,
    string? ContactName = null,
    string? ContactEmail = null,
    string? ContactPhone = null) : ICommand<OrganizationLocationDto>;

public record UpdateOrganizationLocationCommand(
    Guid Id,
    string Name,
    string Address,
    string City,
    string State,
    string Country,
    string PostalCode,
    string? ContactName = null,
    string? ContactEmail = null,
    string? ContactPhone = null,
    double? Latitude = null,
    double? Longitude = null,
    string? Timezone = null) : ICommand<OrganizationLocationDto>;

public record DeleteOrganizationLocationCommand(Guid OrganizationId, Guid LocationId) : ICommand;

public record SetPrimaryLocationCommand(Guid OrganizationId, Guid LocationId) : ICommand;

public record CreateSupplierCommand(
    Guid OrganizationId,
    string Name,
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? Address = null,
    string? TaxId = null,
    string? RegistrationNumber = null,
    string? Website = null) : ICommand<SupplierDto>;

public record UpdateSupplierCommand(
    Guid Id,
    string Name,
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? Address = null,
    string? TaxId = null,
    string? RegistrationNumber = null,
    string? Website = null) : ICommand<SupplierDto>;

public record DeleteSupplierCommand(Guid OrganizationId, Guid SupplierId) : ICommand;

public record UpdateSupplierStatusCommand(Guid Id, string Status) : ICommand;

public record AssignSupplierAssessorCommand(Guid SupplierId, Guid AssessorId) : ICommand;