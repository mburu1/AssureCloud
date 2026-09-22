using System;
using System.Text.RegularExpressions;

namespace AssureCloud.Domain.ValueObjects;

public class PhoneNumber : IEquatable<PhoneNumber>
{
    public string Value { get; private set; } = string.Empty;
    public string CountryCode { get; private set; } = string.Empty;
    public string NationalNumber { get; private set; } = string.Empty;

    private PhoneNumber() { }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var cleaned = CleanPhoneNumber(value);
        if (!IsValid(cleaned))
            throw new ArgumentException("Invalid phone number format", nameof(value));

        Value = cleaned;
        ParsePhoneNumber(cleaned);
    }

    private static string CleanPhoneNumber(string phoneNumber)
    {
        return Regex.Replace(phoneNumber, @"[\s\-\(\)\.]", "");
    }

    private static bool IsValid(string phoneNumber)
    {
        // E.164 format: +[country code][national number]
        return Regex.IsMatch(phoneNumber, @"^\+\d{1,3}\d{4,14}$");
    }

    private void ParsePhoneNumber(string phoneNumber)
    {
        // Simple parsing - in production, use a library like libphonenumber
        if (phoneNumber.StartsWith("+1"))
        {
            CountryCode = "1";
            NationalNumber = phoneNumber.Substring(2);
        }
        else if (phoneNumber.StartsWith("+44"))
        {
            CountryCode = "44";
            NationalNumber = phoneNumber.Substring(3);
        }
        else
        {
            // Generic parsing
            var match = Regex.Match(phoneNumber, @"^\+(\d{1,3})(\d+)$");
            if (match.Success)
            {
                CountryCode = match.Groups[1].Value;
                NationalNumber = match.Groups[2].Value;
            }
        }
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as PhoneNumber);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right) => Equals(left, right);
    public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => !Equals(left, right);

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    public static implicit operator PhoneNumber(string value) => new PhoneNumber(value);
}