using System;

namespace AssureCloud.Domain.ValueObjects;

public class Email : IEquatable<Email>
{
    public string Value { get; private set; } = string.Empty;

    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        if (!IsValid(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value.Trim().ToLowerInvariant();
    }

    public static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as Email);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Email? left, Email? right) => Equals(left, right);
    public static bool operator !=(Email? left, Email? right) => !Equals(left, right);

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new Email(value);
}