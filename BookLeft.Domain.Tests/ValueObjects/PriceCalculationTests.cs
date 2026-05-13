namespace BookRight.Domain.Tests.ValueObjects;

using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class PriceCalculationTests
{
    // Reusable base price of 500 DKK used across all tests
    private static Money Base500 => new Money(500m);

    // Ensures that passing null as base price throws immediately
    [Fact]
    public void Create_NullBasePrice_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            PriceCalculation.Create(null!, LoyaltyLevel.None, false, false, null));
    }

    // Campaign discount cannot be negative — tests two negative values
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Create_NegativeCampaignDiscount_ThrowsArgumentException(decimal discount)
    {
        Assert.Throws<ArgumentException>(() =>
            PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, discount));
    }

    // Campaign discount cannot exceed 100% — tests two values above the limit
    [Theory]
    [InlineData(101)]
    [InlineData(200)]
    public void Create_CampaignDiscountAbove100_ThrowsArgumentException(decimal discount)
    {
        Assert.Throws<ArgumentException>(() =>
            PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, discount));
    }

    // Single test ensuring Gold loyalty gives exactly 15% on 500 kr → 75 kr discount
    [Fact]
    public void Create_GoldLoyalty_Applies15PercentDiscount()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.Gold, false, false, null);
        Assert.Equal(new Money(75m), result.DiscountAmount);
    }

    // Single test ensuring Silver loyalty gives exactly 10% on 500 kr → 50 kr discount
    [Fact]
    public void Create_SilverLoyalty_Applies10PercentDiscount()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.Silver, false, false, null);
        Assert.Equal(new Money(50m), result.DiscountAmount);
    }

    // Single test ensuring Bronze loyalty gives exactly 5% on 500 kr → 25 kr discount
    [Fact]
    public void Create_BronzeLoyalty_Applies5PercentDiscount()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.Bronze, false, false, null);
        Assert.Equal(new Money(25m), result.DiscountAmount);
    }

    // Standard (None) loyalty means no discount applies
    [Fact]
    public void Create_StandardLoyalty_AppliesZeroDiscount()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, null);
        Assert.Equal(new Money(0m), result.DiscountAmount);
    }

    // Birthday month gives 25% discount — 500 kr × 25% = 125 kr
    [Fact]
    public void Create_BirthdayMonth_Applies25PercentDiscount()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, true, false, null);
        Assert.Equal(new Money(125m), result.DiscountAmount);
    }

    // Campaign discount of 10% on 500 kr should give 50 kr discount
    [Fact]
    public void Create_CampaignDiscount_IsAppliedCorrectly()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, 10m);
        Assert.Equal(new Money(50m), result.DiscountAmount);
    }

    // Only the best discount wins — Birthday 25% beats Gold 15%
    [Fact]
    public void Create_BirthdayBeatsLoyalty_WhenBirthdayIsHigher()
    {
        // Birthday 25% > Gold loyalty 15% → Birthday wins
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.Gold, true, false, null);
        Assert.Equal(DiscountType.BirthdayMonth, result.AppliedDiscountType);
        Assert.Equal(new Money(125m), result.DiscountAmount);
    }

    // Only the best discount wins — Gold 15% beats Campaign 5%
    [Fact]
    public void Create_LoyaltyBeatsCampaign_WhenLoyaltyIsHigher()
    {
        // Gold loyalty 15% > Campaign 5% → Loyalty wins
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.Gold, false, false, 5m);
        Assert.Equal(DiscountType.Gold, result.AppliedDiscountType);
        Assert.Equal(new Money(75m), result.DiscountAmount);
    }

    // Only the best discount wins — Birthday 25% beats Campaign 20%
    [Fact]
    public void Create_BirthdayBeatsCampaign_WhenBirthdayIsHigher()
    {
        // Birthday 25% > Campaign 20% → Birthday wins
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, true, false, 20m);
        Assert.Equal(DiscountType.BirthdayMonth, result.AppliedDiscountType);
        Assert.Equal(new Money(125m), result.DiscountAmount);
    }

    // Evening/weekend supplement is 15% of BasePrice — independent of any discount
    // 500 kr × 15% = 75 kr supplement
    [Fact]
    public void Create_EveningOrWeekend_Adds15PercentSupplementOnBasePrice()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, true, null);
        Assert.Equal(new Money(75m), result.Supplement);
    }

    // No supplement when booking is not in the evening or weekend
    [Fact]
    public void Create_NotEveningOrWeekend_NoSupplement()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, null);
        Assert.Equal(new Money(0m), result.Supplement);
    }

    // Full price calculation: BasePrice - Discount + Supplement
    // 500 - 25 (Bronze 5%) + 75 (evening 15%) = 550 kr
    [Fact]
    public void Create_FinalPrice_EqualsBasePriceMinusDiscountPlusSupplement()
    {
        var result = PriceCalculation.Create(new Money(500m), LoyaltyLevel.Bronze, false, true, null);
        Assert.Equal(new Money(550m), result.FinalPrice);
    }

    // When no discounts apply at all, type should be None
    [Fact]
    public void Create_NoDiscounts_AppliedDiscountTypeIsNone()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, null);
        Assert.Equal(DiscountType.None, result.AppliedDiscountType);
    }

    // When birthday wins, the applied type should reflect that
    [Fact]
    public void Create_BirthdayWins_AppliedDiscountTypeIsBirthdayMonth()
    {
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, true, false, null);
        Assert.Equal(DiscountType.BirthdayMonth, result.AppliedDiscountType);
    }

    // Runs three times — once per loyalty level — verifying the correct DiscountType is set
    [Theory]
    [InlineData(LoyaltyLevel.Bronze, DiscountType.Bronze)]
    [InlineData(LoyaltyLevel.Silver, DiscountType.Silver)]
    [InlineData(LoyaltyLevel.Gold,   DiscountType.Gold)]
    public void Create_LoyaltyWins_AppliedDiscountTypeMatchesLoyaltyLevel(
        LoyaltyLevel loyalty, DiscountType expected)
    {
        var result = PriceCalculation.Create(Base500, loyalty, false, false, null);
        Assert.Equal(expected, result.AppliedDiscountType);
    }

    // When no loyalty or birthday applies, campaign discount should win
    [Fact]
    public void Create_CampaignWins_AppliedDiscountTypeIsCampaign()
    {
        // No loyalty, no birthday, campaign 30% → Campaign wins
        var result = PriceCalculation.Create(Base500, LoyaltyLevel.None, false, false, 30m);
        Assert.Equal(DiscountType.Campaign, result.AppliedDiscountType);
    }
}