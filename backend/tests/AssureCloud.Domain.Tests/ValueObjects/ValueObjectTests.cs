using System;
using AssureCloud.Domain.ValueObjects;
using Xunit;

namespace AssureCloud.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldCreateEmail()
    {
        // Arrange
        var emailAddress = "test@example.com";

        // Act
        var email = Email.Create(emailAddress);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(emailAddress.ToLowerInvariant(), email.Value);
    }

    [Fact]
    public void Create_WithUpperCaseEmail_ShouldNormalizeToLowerCase()
    {
        // Arrange
        var emailAddress = "TEST@EXAMPLE.COM";

        // Act
        var email = Email.Create(emailAddress);

        // Assert
        Assert.Equal("test@example.com", email.Value);
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var emailAddress = "invalid-email";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Email.Create(emailAddress));
    }

    [Fact]
    public void Create_WithNullEmail_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Email.Create(null!));
    }

    [Fact]
    public void Equals_TwoEmailsWithSameValue_ShouldBeEqual()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
    }

    [Fact]
    public void ImplicitConversion_ShouldWork()
    {
        // Arrange
        var email = Email.Create("test@example.com");

        // Act
        string value = email;

        // Assert
        Assert.Equal("test@example.com", value);
    }
}

public class PhoneNumberTests
{
    [Fact]
    public void Create_WithValidPhoneNumber_ShouldCreatePhoneNumber()
    {
        // Arrange
        var phoneNumber = "+1 (555) 123-4567";

        // Act
        var phone = PhoneNumber.Create(phoneNumber);

        // Assert
        Assert.NotNull(phone);
        Assert.Equal(phoneNumber, phone.Value);
    }

    [Fact]
    public void Create_WithInvalidPhoneNumber_ShouldThrowArgumentException()
    {
        // Arrange
        var phoneNumber = "invalid";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(phoneNumber));
    }

    [Fact]
    public void Equals_TwoPhoneNumbersWithSameValue_ShouldBeEqual()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("+15551234567");
        var phone2 = PhoneNumber.Create("+15551234567");

        // Act & Assert
        Assert.Equal(phone1, phone2);
    }
}

public class AddressTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAddress()
    {
        // Arrange
        var street = "123 Main St";
        var city = "New York";
        var state = "NY";
        var country = "USA";
        var postalCode = "10001";

        // Act
        var address = Address.Create(street, city, state, country, postalCode);

        // Assert
        Assert.NotNull(address);
        Assert.Equal(street, address.Street);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(country, address.Country);
        Assert.Equal(postalCode, address.PostalCode);
    }

    [Fact]
    public void Create_WithNullStreet_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Address.Create(null!, "City", "State", "Country", "12345"));
    }

    [Fact]
    public void ToString_ShouldReturnFormattedAddress()
    {
        // Arrange
        var address = Address.Create("123 Main St", "New York", "NY", "USA", "10001");

        // Act
        var formatted = address.ToString();

        // Assert
        Assert.Equal("123 Main St, New York, NY, USA 10001", formatted);
    }

    [Fact]
    public void Equals_TwoAddressesWithSameValues_ShouldBeEqual()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("123 Main St", "New York", "NY", "USA", "10001");

        // Act & Assert
        Assert.Equal(address1, address2);
    }
}

public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmountAndCurrency_ShouldCreateMoney()
    {
        // Arrange
        var amount = 100.50m;
        var currency = "USD";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.NotNull(money);
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Money.Create(-10m, "USD"));
    }

    [Fact]
    public void Create_WithEmptyCurrency_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Money.Create(100m, ""));
    }

    [Fact]
    public void Add_TwoMoneysWithSameCurrency_ShouldReturnSum()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(50m, "USD");

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(150m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Add_TwoMoneysWithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(50m, "EUR");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1.Add(money2));
    }

    [Fact]
    public void Subtract_TwoMoneysWithSameCurrency_ShouldReturnDifference()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(30m, "USD");

        // Act
        var result = money1.Subtract(money2);

        // Assert
        Assert.Equal(70m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Multiply_MoneyByFactor_ShouldReturnProduct()
    {
        // Arrange
        var money = Money.Create(100m, "USD");

        // Act
        var result = money.Multiply(1.5m);

        // Assert
        Assert.Equal(150m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Equals_TwoMoneysWithSameAmountAndCurrency_ShouldBeEqual()
    {
        // Arrange
        var money1 = Money.Create(100m, "USD");
        var money2 = Money.Create(100m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
    }
}