using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class User : BaseEntity
{
    private readonly List<Guid> _roleIds = new();

    public Guid OrganizationId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Active;
    public DateTime? LastLoginAt { get; private set; }
    public string? AvatarUrl { get; private set; }
    public string? Timezone { get; private set; }
    public string? Language { get; private set; }

    public IReadOnlyCollection<Guid> RoleIds => _roleIds.AsReadOnly();

    public string FullName => $"{FirstName} {LastName}";

    private User() { }

    public User(
        Guid organizationId,
        string email,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        Guid? roleId = null)
    {
        OrganizationId = organizationId;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        PhoneNumber = phoneNumber;

        if (roleId.HasValue)
        {
            _roleIds.Add(roleId.Value);
        }

        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string? phoneNumber = null,
        string? avatarUrl = null,
        string? timezone = null,
        string? language = null)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        PhoneNumber = phoneNumber;
        AvatarUrl = avatarUrl;
        Timezone = timezone;
        Language = language;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(UserStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void AddRole(Guid roleId)
    {
        if (!_roleIds.Contains(roleId))
        {
            _roleIds.Add(roleId);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveRole(Guid roleId)
    {
        _roleIds.Remove(roleId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRoles(IEnumerable<Guid> roleIds)
    {
        _roleIds.Clear();
        _roleIds.AddRange(roleIds);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasRole(Guid roleId) => _roleIds.Contains(roleId);
}

public class UserStatus : Enumeration
{
    public static readonly UserStatus Active = new(0, "Active");
    public static readonly UserStatus Inactive = new(1, "Inactive");
    public static readonly UserStatus Locked = new(2, "Locked");
    public static readonly UserStatus PendingVerification = new(3, "PendingVerification");
    public static readonly UserStatus Suspended = new(4, "Suspended");

    private UserStatus(int value, string name) : base(value, name) { }
}