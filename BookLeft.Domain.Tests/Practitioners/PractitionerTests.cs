namespace BookRight.Domain.Tests.Practitioners;

using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Exceptions;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class PractitionerTests
{
    // Reusable helper that creates a valid practitioner — avoids repeating constructor calls in every test
    private static Practitioner CreateValidPractitioner(
        AuthorizationType authType = AuthorizationType.Physiotherapist) =>
        new Practitioner("Hans Andersen", "hans@klinik.dk", "12345678", "AUTH123", authType);

    // Full happy path — all valid data should produce a practitioner with correct property values
    [Fact]
    public void Constructor_ValidData_CreatesPractitioner()
    {
        var p = CreateValidPractitioner();
        Assert.Equal("Hans Andersen", p.Name);
        Assert.Equal("hans@klinik.dk", p.Email);
        Assert.Equal("12345678", p.PhoneNumber);
        Assert.Equal("AUTH123", p.AuthorizationCode);
        Assert.Equal(AuthorizationType.Physiotherapist, p.AuthorizationType);
    }

    // Email must follow standard format — tests three common invalid formats
    [Theory]
    [InlineData("notvalid")]      // no @ symbol
    [InlineData("no@domain")]     // no TLD dot after domain
    [InlineData("@domain.com")]   // empty local part
    public void Constructor_InvalidEmail_ThrowsDomainException(string email)
    {
        Assert.Throws<DomainException>(() =>
            new Practitioner("Hans Andersen", email, "12345678", "AUTH123",
                AuthorizationType.Physiotherapist));
    }

    // Phone number must be digits only and at least 8 characters
    [Theory]
    [InlineData("1234")]          // fewer than 8 digits
    [InlineData("abc12345678")]   // contains letters
    public v