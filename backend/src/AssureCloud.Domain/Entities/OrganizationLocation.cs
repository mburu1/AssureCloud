using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class OrganizationLocation : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public string? Timezone { get; private set; }
    public string? ContactName { get; private set; }
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }

    private OrganizationLocation() { }

    public OrganizationLocation(
        Guid organizationId,
        string name,
        string address,
        string city,
        string state,
        string country,
        string postalCode,
        bool isPrimary = false)
    {
        OrganizationId = organizationId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
        IsPrimary = isPrimary;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string address,
        string city,
        string state,
        string country,
        string postalCode,
        string? contactName = null,
        string? contactEmail = null,
        string? contactPhone = null,
        double? latitude = null,
        double? longitude = null,
        string? timezone = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
        ContactName = contactName;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsNonPrimary()
    {
        IsPrimary = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCoordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTime.UtcNow;
    }
}