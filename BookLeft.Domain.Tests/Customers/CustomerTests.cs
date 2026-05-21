namespace BookRight.Domain.Tests.Customers;

using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class CustomerTests
{
    // Fixed date of birth used across all tests — avoids repeating the same value
    private static readonly DateTime ValidDob = new DateTime(1990, 6, 15);

    // Reusable helper that creates a valid customer — note is optional and defaults to null
    private static Customer CreateValidCustomer(string? note = null) =>
        new Customer("Ane Nielsen", "12345678", "ane@mail.dk",
            LoyaltyLevel.None, ValidDob, note, "Testgade 1", "Vejle", "7100");

    // Full happy path — all valid data should produce a customer with correct property values
    [Fact]
    public void Constructor_ValidData_CreatesCustomer()
    {
        var customer = CreateValidCustomer();
        Assert.Equal("Ane Nielsen", customer.Name);
        Assert.Equal("12345678", customer.PhoneNumber);
        Assert.Equal("ane@mail.dk", customer.Email);
        Assert.Equal(LoyaltyLevel.None, customer.LoyaltyLevel);
        Assert.Equal(ValidDob, customer.DateOfBirth);
        Assert.Null(customer.Note);
    }

    // Name is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_ThrowsArgumentException(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer(name, "12345678", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Email is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyEmail_ThrowsArgumentException(string email)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", email,
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Phone number is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyPhoneNumber_ThrowsArgumentException(string phone)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", phone, "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Phone number must contain digits only — letters mixed in should throw
    [Theory]
    [InlineData("1234567A")]   // letter mixed with digits
    [InlineData("12A45678")]
    public void Constructor_PhoneNumberContainingNonDigits_ThrowsArgumentException(string phone)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", phone, "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Phone number must be at least 8 digits — 7 digits should throw
    [Fact]
    public void Constructor_PhoneNumberLessThan8Digits_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "1234567", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Phone number cannot exceed 11 digits — 12 digits should throw
    [Fact]
    public void Constructor_PhoneNumberMoreThan11Digits_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "123456789012", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", "7100"));
    }

    // Date of birth cannot be in the future — a customer must already exist
    [Fact]
    public void Constructor_DateOfBirthInFuture_ThrowsArgumentException()
    {
        var future = DateTime.Now.AddDays(1);
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", "ane@mail.dk",
                LoyaltyLevel.None, future, null, "Testgade 1", "Vejle", "7100"));
    }

    // Danish zipcodes are always exactly 4 digits — too short or too long should throw
    [Theory]
    [InlineData("123")]     // 3 digits — too short
    [InlineData("12345")]   // 5 digits — too long
    public void Constructor_ZipcodeNotFourDigits_ThrowsArgumentException(string zipcode)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", zipcode));
    }

    // Zipcode must contain digits only — letters should throw
    [Theory]
    [InlineData("710A")]
    [InlineData("ABCD")]
    public void Constructor_ZipcodeContainingNonDigits_ThrowsArgumentException(string zipcode)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", "Vejle", zipcode));
    }

    // City is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyCity_ThrowsArgumentException(string city)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, "Testgade 1", city, "7100"));
    }

    // Street is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyStreet_ThrowsArgumentException(string street)
    {
        Assert.Throws<ArgumentException>(() =>
            new Customer("Ane", "12345678", "ane@mail.dk",
                LoyaltyLevel.None, ValidDob, null, street, "Vejle", "7100"));
    }

    // Note is optional — null should be stored without throwing
    [Fact]
    public void Constructor_NullNote_DoesNotThrow()
    {
        var customer = CreateValidCustomer(note: null);
        Assert.Null(customer.Note);
    }
}