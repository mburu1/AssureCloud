using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Organizations;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Organizations;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Organizations;

public class OrganizationService :
    ICommandHandler<CreateOrganizationCommand, OrganizationDto>,
    ICommandHandler<UpdateOrganizationCommand, OrganizationDto>,
    ICommandHandler<DeleteOrganizationCommand>,
    ICommandHandler<UpdateOrganizationStatusCommand>,
    ICommandHandler<CreateOrganizationLocationCommand, OrganizationLocationDto>,
    ICommandHandler<UpdateOrganizationLocationCommand, OrganizationLocationDto>,
    ICommandHandler<DeleteOrganizationLocationCommand>,
    ICommandHandler<SetPrimaryLocationCommand>,
    ICommandHandler<CreateSupplierCommand, SupplierDto>,
    ICommandHandler<UpdateSupplierCommand, SupplierDto>,
    ICommandHandler<DeleteSupplierCommand>,
    ICommandHandler<UpdateSupplierStatusCommand>,
    ICommandHandler<AssignSupplierAssessorCommand>,
    IQueryHandler<GetOrganizationByIdQuery, OrganizationDto?>,
    IQueryHandler<GetOrganizationsQuery, PaginatedResult<OrganizationListDto>>,
    IQueryHandler<GetOrganizationLocationsQuery, IReadOnlyList<OrganizationLocationDto>>,
    IQueryHandler<GetOrganizationLocationByIdQuery, OrganizationLocationDto?>,
    IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>,
    IQueryHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IRepository<OrganizationLocation> _locationRepository;
    private readonly IRepository<Supplier> _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrganizationService(
        IRepository<Organization> organizationRepository,
        IRepository<OrganizationLocation> locationRepository,
        IRepository<Supplier> supplierRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _locationRepository = locationRepository;
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrganizationDto> HandleAsync(CreateOrganizationCommand command, CancellationToken ct = default)
    {
        var organization = new Organization(
            command.Name,
            command.Description,
            command.RegistrationNumber,
            command.TaxId);

        if (!string.IsNullOrEmpty(command.Website))
        {
            organization.UpdateDetails(organization.Name, organization.Description, command.Website, organization.LogoUrl);
        }

        await _organizationRepository.AddAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<OrganizationDto>(organization);
    }

    public async Task<OrganizationDto> HandleAsync(UpdateOrganizationCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Organization));

        organization.UpdateDetails(command.Name, command.Description, command.Website, command.LogoUrl);
        await _organizationRepository.UpdateAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<OrganizationDto>(organization);
    }

    public async Task HandleAsync(DeleteOrganizationCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Organization));

        organization.MarkAsDeleted();
        await _organizationRepository.UpdateAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateOrganizationStatusCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Organization));

        if (Enum.TryParse<OrganizationStatus>(command.Status, out var status))
        {
            organization.UpdateStatus(status);
            await _organizationRepository.UpdateAsync(organization, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task<OrganizationLocationDto> HandleAsync(CreateOrganizationLocationCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.OrganizationId, nameof(Organization));

        var location = organization.AddLocation(
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.Country,
            command.PostalCode,
            command.IsPrimary);

        if (command.Latitude.HasValue && command.Longitude.HasValue)
        {
            location.SetCoordinates(command.Latitude.Value, command.Longitude.Value);
        }

        if (!string.IsNullOrEmpty(command.Timezone))
        {
            location.UpdateDetails(
                location.Name, location.Address, location.City, location.State,
                location.Country, location.PostalCode,
                command.ContactName, command.ContactEmail, command.ContactPhone,
                location.Latitude, location.Longitude, command.Timezone);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<OrganizationLocationDto>(location);
    }

    public async Task<OrganizationLocationDto> HandleAsync(UpdateOrganizationLocationCommand command, CancellationToken ct = default)
    {
        var location = await _locationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(OrganizationLocation));

        location.UpdateDetails(
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.Country,
            command.PostalCode,
            command.ContactName,
            command.ContactEmail,
            command.ContactPhone,
            command.Latitude,
            command.Longitude,
            command.Timezone);

        await _locationRepository.UpdateAsync(location, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<OrganizationLocationDto>(location);
    }

    public async Task HandleAsync(DeleteOrganizationLocationCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.OrganizationId, nameof(Organization));

        organization.RemoveLocation(command.LocationId);
        await _organizationRepository.UpdateAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetPrimaryLocationCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.OrganizationId, nameof(Organization));

        var location = organization.Locations.FirstOrDefault(l => l.Id == command.LocationId)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.LocationId, nameof(OrganizationLocation));

        foreach (var loc in organization.Locations)
        {
            loc.SetAsNonPrimary();
        }

        location.SetAsPrimary();
        await _organizationRepository.UpdateAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<SupplierDto> HandleAsync(CreateSupplierCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.OrganizationId, nameof(Organization));

        var supplier = organization.AddSupplier(
            command.Name,
            command.ContactEmail,
            command.ContactPhone,
            command.Address);

        if (!string.IsNullOrEmpty(command.TaxId))
        {
            supplier.UpdateDetails(
                supplier.Name,
                supplier.ContactEmail,
                supplier.ContactPhone,
                supplier.Address,
                command.TaxId,
                command.RegistrationNumber,
                command.Website);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<SupplierDto> HandleAsync(UpdateSupplierCommand command, CancellationToken ct = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Supplier));

        supplier.UpdateDetails(
            command.Name,
            command.ContactEmail,
            command.ContactPhone,
            command.Address,
            command.TaxId,
            command.RegistrationNumber,
            command.Website);

        await _supplierRepository.UpdateAsync(supplier, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task HandleAsync(DeleteSupplierCommand command, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.OrganizationId, nameof(Organization));

        organization.RemoveSupplier(command.SupplierId);
        await _organizationRepository.UpdateAsync(organization, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateSupplierStatusCommand command, CancellationToken ct = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Supplier));

        if (Enum.TryParse<SupplierStatus>(command.Status, out var status))
        {
            supplier.UpdateStatus(status);
            await _supplierRepository.UpdateAsync(supplier, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(AssignSupplierAssessorCommand command, CancellationToken ct = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.SupplierId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.SupplierId, nameof(Supplier));

        supplier.AssignAssessor(command.AssessorId);
        await _supplierRepository.UpdateAsync(supplier, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<OrganizationDto?> HandleAsync(GetOrganizationByIdQuery query, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(query.Id, ct);
        return organization != null ? _mapper.Map<OrganizationDto>(organization) : null;
    }

    public async Task<PaginatedResult<OrganizationListDto>> HandleAsync(GetOrganizationsQuery query, CancellationToken ct = default)
    {
        var organizations = await _organizationRepository.ListAsync(ct);

        var filtered = organizations.AsQueryable();

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            filtered = filtered.Where(o =>
                o.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (o.Description != null && o.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (o.RegistrationNumber != null && o.RegistrationNumber.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<OrganizationStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(o => o.Status == status);
        }

        if (query.ParentOrganizationId.HasValue)
        {
            filtered = filtered.Where(o => o.ParentOrganizationId == query.ParentOrganizationId.Value);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderBy(o => o.Name)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<OrganizationListDto>>(items);

        return new PaginatedResult<OrganizationListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IReadOnlyList<OrganizationLocationDto>> HandleAsync(GetOrganizationLocationsQuery query, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(query.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.OrganizationId, nameof(Organization));

        return _mapper.Map<IReadOnlyList<OrganizationLocationDto>>(organization.Locations);
    }

    public async Task<OrganizationLocationDto?> HandleAsync(GetOrganizationLocationByIdQuery query, CancellationToken ct = default)
    {
        var location = await _locationRepository.GetByIdAsync(query.LocationId, ct);
        return location != null ? _mapper.Map<OrganizationLocationDto>(location) : null;
    }

    public async Task<IReadOnlyList<SupplierDto>> HandleAsync(GetSuppliersQuery query, CancellationToken ct = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(query.OrganizationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.OrganizationId, nameof(Organization));

        return _mapper.Map<IReadOnlyList<SupplierDto>>(organization.Suppliers);
    }

    public async Task<SupplierDto?> HandleAsync(GetSupplierByIdQuery query, CancellationToken ct = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(query.SupplierId, ct);
        return supplier != null ? _mapper.Map<SupplierDto>(supplier) : null;
    }
}