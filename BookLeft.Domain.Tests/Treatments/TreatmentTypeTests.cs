namespace BookRight.Domain.Tests.Treatments;

using BookRight.Domain.Entities.Treatments;
using BookRight.Domain.ValueObjects;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class TreatmentTypeTests
{
    // Reusable valid price used across tests — mirrors a real fysioterapi 45 min price
    private static Money ValidPrice => new Money(395m);

    // Full happy path — all valid data should create a treatment with correct values
    [Fact]
    public void Constructor_ValidData_CreatesTreatmentType()
    {
        var treatment = new TreatmentType(
            "Fysioterapi", 45, ValidPrice, AuthorizationType.Physiotherapist, 1);
        Assert.Equal("Fysioterapi", treatment.Name);
        Assert.Equal(45, treatment.DurationMinutes);
        Assert.Equal(ValidPrice, treatment.BasePrice);
        Assert.Equal(AuthorizationType.Physiotherapist, treatment.NeedsAuthorisation);
        Assert.Equal(1, treatment.MaxParticipants);
    }

    // A treatment must have a name — null should throw immediately
    [Fact]
    public void Constructor_NullName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new TreatmentType(null!, 45, ValidPrice, AuthorizationType.Physiotherapist, 1));
    }

    // Duration must be a positive number — 0 and negative values are not valid treatment lengths
    [Theory]
    [InlineData(0)]
    [InlineData(-30)]
    public void Constructor_ZeroOrNegativeDuration_ThrowsArgumentException(int duration)
    {
        Assert.Throws<ArgumentException>(() =>
            new TreatmentType("Fysioterapi", duration, ValidPrice, AuthorizationType.Physiotherapist, 1));
    }

    // BasePrice is required — Money handles its own validation so null throws ArgumentNullException
    [Fact]
    public void Constructor_NullBasePrice_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TreatmentType("Fysioterapi", 45, null!, AuthorizationType.Physiotherapist, 1));
    }

    // MaxParticipants must be at least 1 — 0 and negative values make no sense
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ZeroOrNegativeMaxParticipants_ThrowsArgumentException(int max)
    {
        Assert.Throws<ArgumentException>(() =>
            new TreatmentType("Holdtræning", 60, ValidPrice, AuthorizationType.Physiotherapist, max));
    }

    // Solo treatments like Akupunktur have exactly 1 participant — must be valid
    [Fact]
    public void Constructor_OneMaxParticipant_IsValid()
    {
        var treatment = new TreatmentType(
            "Akupunktur", 45, ValidPrice, AuthorizationType.Acupuncturist, 1);
        Assert.Equal(1, treatment.MaxParticipants);
    }

    // Group treatments like Holdtræning allow up to 6 participants — must be valid
    [Fact]
    public void Constructor_SixMaxParticipants_IsValid()
    {
        var treatment = new TreatmentType(
            "Holdtræning", 60, ValidPrice, AuthorizationType.Physiotherapist, 6);
        Assert.Equal(6, treatment.MaxParticipants);
    }
}