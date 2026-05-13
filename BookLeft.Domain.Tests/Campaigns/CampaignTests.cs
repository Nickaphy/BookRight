namespace BookRight.Domain.Tests.Campaigns;

using Bookright.Domain.Campaigns;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class CampaignTests
{
    // Helper that returns today as DateOnly — used as a base for all date calculations in tests
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    // Full happy path — all valid data should produce a campaign with correct property values
    [Fact]
    public void Constructor_ValidData_CreatesCampaign()
    {
        var start = Today;
        var end = Today.AddDays(30);
        var campaign = new Campaign("Sommertilbud", start, end, 20);
        Assert.Equal("Sommertilbud", campaign.Name);
        Assert.Equal(start, campaign.StartDate);
        Assert.Equal(end, campaign.EndDate);
        Assert.Equal(20, campaign.discountPercent);
    }

    // Campaign name is required — empty string and whitespace should both throw
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyName_ThrowsArgumentException(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new Campaign(name, Today, Today.AddDays(30), 20));
    }

    // A campaign cannot end before it starts — start date after end date should throw
    [Fact]
    public void Constructor_StartDateAfterEndDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Campaign("Test", Today.AddDays(10), Today, 20));
    }

    // Discount cannot be negative — tests two negative values
    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Constructor_DiscountPercentBelow0_ThrowsArgumentException(int discount)
    {
        Assert.Throws<ArgumentException>(() =>
            new Campaign("Test", Today, Today.AddDays(30), discount));
    }

    // Discount cannot exceed 100% — tests two values above the limit
    [Theory]
    [InlineData(101)]
    [InlineData(200)]
    public void Constructor_DiscountPercentAbove100_ThrowsArgumentException(int discount)
    {
        Assert.Throws<ArgumentException>(() =>
            new Campaign("Test", Today, Today.AddDays(30), discount));
    }

    // 0% is a valid discount — e.g. a placeholder campaign with no active discount yet
    [Fact]
    public void Constructor_ZeroDiscountPercent_IsValid()
    {
        var campaign = new Campaign("Test", Today, Today.AddDays(30), 0);
        Assert.Equal(0, campaign.discountPercent);
    }

    // 100% is a valid discount — e.g. a fully free promotional campaign
    [Fact]
    public void Constructor_100DiscountPercent_IsValid()
    {
        var campaign = new Campaign("Test", Today, Today.AddDays(30), 100);
        Assert.Equal(100, campaign.discountPercent);
    }
}