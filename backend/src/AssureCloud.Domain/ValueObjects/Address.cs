using System;

namespace AssureCloud.Domain.ValueObjects;

public class Address : IEquatable<Address>
{
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;

    private Address() { }

    public Address(string street, string city, string state, string country, string postalCode)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
    }

    public void Update(string street, string city, string state, string country, string postalCode)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
    }

    public string GetFullAddress() => $"{Street}, {City}, {State}, {Country} {PostalCode}";

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               Country == other.Country &&
               PostalCode == other.PostalCode;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() => HashCode.Combine(Street, City, State, Country, PostalCode);

    public static bool operator ==(Address? left, Address? right) => Equals(left, right);
    public static bool operator !=(Address? left, Address? right) => !Equals(left, right);

    public override string ToString() => GetFullAddress();
}