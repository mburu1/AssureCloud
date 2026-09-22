using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Supplier : BaseEntity
{
    private readonly List<Organization> _organizations = new();

    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? Address { get; private set; }
    public string? TaxId { get; private set; }
    public string? RegistrationNumber { get; private set; }
    public string? Website { get; private set; }
    public SupplierStatus Status { get; private set; } = SupplierStatus.Active;
    public Guid? AssignedAssessorId { get; private set; }
    public DateTime? LastAssessmentDate { get; private set; }

    public IReadOnlyCollection<Organization> Organizations => _organizations.AsReadOnly();

    private Supplier() { }

    public Supplier(
        Guid organizationId,
        string name,
        string? contactEmail = null,
        string? contactPhone = null,
        string? address = null)
    {
        OrganizationId = organizationId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Address = address;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string name,
        string? contactEmail = null,
        string? contactPhone = null,
        string? address = null,
        string? taxId = null,
        string? registrationNumber = null,
        string? website = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Address = address;
        TaxId = taxId;
        RegistrationNumber = registrationNumber;
        Website = website;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(SupplierStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignAssessor(Guid assessorId)
    {
        AssignedAssessorId = assessorId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordAssessmentCompletion()
    {
        LastAssessmentDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class SupplierStatus : Enumeration
{
    public static readonly SupplierStatus Active = new(0, "Active");
    public static readonly SupplierStatus Inactive = new(1, "Inactive");
    public static readonly SupplierStatus PendingReview = new(2, "PendingReview");
    public static readonly SupplierStatus Suspended = new(3, "Suspended");
    public static readonly SupplierStatus Disqualified = new(4, "Disqualified");

    private SupplierStatus(int value, string name) : base(value, name) { }
}