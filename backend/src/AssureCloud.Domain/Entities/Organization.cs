using System;
using System.Collections.Generic;
using AssureCloud.Domain.Enums;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Organization : BaseEntity
{
    private readonly List<OrganizationLocation> _locations = new();
    private readonly List<Supplier> _suppliers = new();
    private readonly List<User> _users = new();

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? RegistrationNumber { get; private set; }
    public string? TaxId { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    public OrganizationStatus Status { get; private set; } = OrganizationStatus.Active;
    public Guid? ParentOrganizationId { get; private set; }
    public Organization? ParentOrganization { get; private set; }

    public IReadOnlyCollection<OrganizationLocation> Locations => _locations.AsReadOnly();
    public IReadOnlyCollection<Supplier> Suppliers => _suppliers.AsReadOnly();
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    private Organization() { }

    public Organization(string name, string? description = null, string? registrationNumber = null, string? taxId = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        RegistrationNumber = registrationNumber;
        TaxId = taxId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description, string? website, string? logoUrl)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Website = website;
        LogoUrl = logoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(OrganizationStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public OrganizationLocation AddLocation(string name, string address, string city, string state, string country, string postalCode, bool isPrimary = false)
    {
        var location = new OrganizationLocation(Id, name, address, city, state, country, postalCode, isPrimary);
        _locations.Add(location);

        if (isPrimary)
        {
            foreach (var loc in _locations.Where(l => l.Id != location.Id))
            {
                loc.SetAsNonPrimary();
            }
        }

        AddDomainEvent(new OrganizationLocationAddedEvent(Id, location.Id));
        return location;
    }

    public void RemoveLocation(Guid locationId)
    {
        var location = _locations.FirstOrDefault(l => l.Id == locationId);
        if (location != null)
        {
            _locations.Remove(location);
            AddDomainEvent(new OrganizationLocationRemovedEvent(Id, locationId));
        }
    }

    public Supplier AddSupplier(string name, string? contactEmail, string? contactPhone, string? address)
    {
        var supplier = new Supplier(Id, name, contactEmail, contactPhone, address);
        _suppliers.Add(supplier);
        AddDomainEvent(new SupplierAddedEvent(Id, supplier.Id));
        return supplier;
    }

    public void RemoveSupplier(Guid supplierId)
    {
        var supplier = _suppliers.FirstOrDefault(s => s.Id == supplierId);
        if (supplier != null)
        {
            _suppliers.Remove(supplier);
            AddDomainEvent(new SupplierRemovedEvent(Id, supplierId));
        }
    }

    public User AddUser(string email, string firstName, string lastName, string? phoneNumber, Guid roleId)
    {
        var user = new User(Id, email, firstName, lastName, phoneNumber, roleId);
        _users.Add(user);
        AddDomainEvent(new UserAddedEvent(Id, user.Id));
        return user;
    }

    public void RemoveUser(Guid userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _users.Remove(user);
            AddDomainEvent(new UserRemovedEvent(Id, userId));
        }
    }

    public void SetParentOrganization(Guid? parentOrganizationId)
    {
        ParentOrganizationId = parentOrganizationId;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class OrganizationStatus : Enumeration
{
    public static readonly OrganizationStatus Active = new(0, "Active");
    public static readonly OrganizationStatus Inactive = new(1, "Inactive");
    public static readonly OrganizationStatus Suspended = new(2, "Suspended");
    public static readonly OrganizationStatus PendingVerification = new(3, "PendingVerification");
    public static readonly OrganizationStatus Archived = new(4, "Archived");

    private OrganizationStatus(int value, string name) : base(value, name) { }
}