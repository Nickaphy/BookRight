using BookRight.Domain.Exceptions;

namespace BookRight.Domain.Tests.Clinics;

using BookRight.Domain.Entities.Clinics;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class ClinicTests
{
    // Reusable helper that creates a valid clinic — rooms is configurable for capacity tests
    private static Clinic CreateValidClinic(int rooms = 3) =>
        new Clinic("Vejle Klinik", rooms, "Testgade 1", "Vejle", "7100");

    // Full happy path — all valid data should produce a clinic with correct property values
    [Fact]
    public void Constructor_ValidData_CreatesClinic()
    {
        var clinic = CreateValidClinic();
        Assert.Equal("Vejle Klinik", clinic.Name);
        Assert.Equal(3, clinic.AmountTreatmentRooms);
        Assert.Equal("Testgade 1", clinic.Street);
        Assert.Equal("Vejle", clinic.City);
        Assert.Equal("7100", clinic.Zipcode);
    }

    // A clinic must have at least one treatment room — 0 and negative values should throw
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ZeroOrNegativeRooms_ThrowsDomainException(int rooms)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic("Vejle Klinik", rooms, "Testgade 1", "Vejle", "7100"));
    }

    // Clinic name is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_ThrowsDomainException(string name)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic(name, 1, "Testgade 1", "Vejle", "7100"));
    }

    // Danish zipcodes are always exactly 4 digits — too short or too long should throw
    [Theory]
    [InlineData("123")]     // 3 digits — too short
    [InlineData("12345")]   // 5 digits — too long
    public void Constructor_ZipcodeNotFourDigits_ThrowsDomainException(string zipcode)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic("Vejle Klinik", 1, "Testgade 1", "Vejle", zipcode));
    }

    // Zipcode must contain digits only — letters should throw
    [Theory]
    [InlineData("710A")]
    [InlineData("AB12")]
    public void Constructor_ZipcodeContainingNonDigits_ThrowsDomainException(string zipcode)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic("Vejle Klinik", 1, "Testgade 1", "Vejle", zipcode));
    }

    // City is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyCity_ThrowsDomainException (string city)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic("Vejle Klinik", 1, "Testgade 1", city, "7100"));
    }

    // Street is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyStreet_ThrowsDomainException(string street)
    {
        Assert.Throws<DomainException>(() =>
            new Clinic("Vejle Klinik", 1, street, "Vejle", "7100"));
    }

    // MaxSimultaneousBookings is derived from AmountTreatmentRooms — one booking per room
    [Fact]
    public void MaxSimultaneousBookings_EqualsAmountTreatmentRooms()
    {
        var clinic = CreateValidClinic(rooms: 5);
        Assert.Equal(clinic.AmountTreatmentRooms, clinic.MaxSimultaneousBookings);
    }

    // OpeningHours should start as an empty list — not null
    [Fact]
    public void Constructor_ValidData_InitializesEmptyOpeningHours()
    {
        var clinic = CreateValidClinic();
        Assert.Empty(clinic.OpeningHours);
    }
}
