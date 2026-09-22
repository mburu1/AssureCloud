using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Organizations;

public record GetOrganizationByIdQuery(Guid Id) : IQuery<OrganizationDto?>;

public record GetOrganizationsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    string? Status = null,
    Guid? ParentOrganizationId = null) : IQuery<PaginatedResult<OrganizationListDto>>;

public record GetOrganizationLocationsQuery(Guid OrganizationId) : IQuery<IReadOnlyList<OrganizationLocationDto>>;

public record GetOrganizationLocationByIdQuery(Guid OrganizationId, Guid LocationId) : IQuery<OrganizationLocationDto?>;

public record GetSuppliersQuery(Guid OrganizationId) : IQuery<IReadOnlyList<SupplierDto>>;

public record GetSupplierByIdQuery(Guid OrganizationId, Guid SupplierId) : IQuery<SupplierDto?>;