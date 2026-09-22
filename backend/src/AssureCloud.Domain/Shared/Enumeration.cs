using System;
using System.Collections.Generic;
using System.Linq;

namespace AssureCloud.Domain;

public abstract class Enumeration : IComparable
{
    public int Value { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    protected Enumeration() { }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = info.GetValue(null);
            if (instance is T locatedValue)
            {
                yield return locatedValue;
            }
        }
    }

    public static T? FromValue<T>(int value) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(e => e.Value == value);
    }

    public static T? FromName<T>(string name) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public static bool TryFromValue<T>(int value, out T? result) where T : Enumeration
    {
        result = FromValue<T>(value);
        return result != null;
    }

    public static bool TryFromName<T>(string name, out T? result) where T : Enumeration
    {
        result = FromName<T>(name);
        return result != null;
    }

    public int CompareTo(object? other)
    {
        if (other is not Enumeration otherValue)
        {
            throw new ArgumentException($"Object is not an {nameof(Enumeration)}");
        }

        return Value.CompareTo(otherValue.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Enumeration? left, Enumeration? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Enumeration? left, Enumeration? right) => !(left == right);
}