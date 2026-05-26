using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.DiscountStrategy;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

// Erik's work.

namespace BookRight.Application.Tests.Pricing;

public class LoyaltyDiscountStrategyTests
{
    [Fact]
    public async Task BronzeStrategy_Returns5PercentDiscount()
    {
        // Arrange
        var strategy = new LoyaltyLevelBronze();

        var context = CreatePricingContext(
            LoyaltyLevel.Bronze,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(50m, discount);
    }

    [Fact]
    public async Task SilverStrategy_Returns10PercentDiscount()
    {
        // Arrange
        var strategy = new LoyaltyLevelSilver();

        var context = CreatePricingContext(
            LoyaltyLevel.Silver,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(100m, discount);
    }

    [Fact]
    public async Task GoldStrategy_Returns15PercentDiscount()
    {
        // Arrange
        var strategy = new LoyaltyLevelGold();

        var context = CreatePricingContext(
            LoyaltyLevel.Gold,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(150m, discount);
    }

    [Fact]
    public async Task BronzeStrategy_ReturnsZero_WhenCustomerIsNotBronze()
    {
        // Arrange
        var strategy = new LoyaltyLevelBronze();

        var context = CreatePricingContext(
            LoyaltyLevel.None,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(0m, discount);
    }

    [Fact]
    public async Task SilverStrategy_ReturnsZero_WhenCustomerIsNotSilver()
    {
        // Arrange
        var strategy = new LoyaltyLevelSilver();

        var context = CreatePricingContext(
            LoyaltyLevel.None,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(0m, discount);
    }

    [Fact]
    public async Task GoldStrategy_ReturnsZero_WhenCustomerIsNotGold()
    {
        // Arrange
        var strategy = new LoyaltyLevelGold();

        var context = CreatePricingContext(
            LoyaltyLevel.None,
            1000m);

        // Act
        var discount = await strategy.CalculateDiscountAsync(context);

        // Assert
        Assert.Equal(0m, discount);
    }

    private static BookingPricingContext CreatePricingContext(
        LoyaltyLevel loyaltyLevel,
        decimal basePrice)
    {
        // Shared helper keeps tests focused on business rules.
        var booking = Booking.Create(
            customerId: Guid.NewGuid(),
            practitionerId: Guid.NewGuid(),
            clinicId: Guid.NewGuid(),
            treatmentTypeId: Guid.NewGuid(),
            timeRange: new TimeRange(
                DateTime.Today.AddDays(1).AddHours(10),
                DateTime.Today.AddDays(1).AddHours(11)),
            basePrice: new Money(basePrice));

        var customer = Customer.Create(
            name: "Test Customer",
            phoneNumber: "12345678",
            email: "test@test.dk",
            loyaltyLevel: loyaltyLevel,
            dateOfBirth: new DateTime(1990, 5, 10),
            note: null,
            street: "Testvej 1",
            city: "Vejle",
            zipcode: "7100");

        return new BookingPricingContext
        {
            Booking = booking,
            Customer = customer,
            IsBirthdayMonth = false,
            HasUsedBirthdayDiscountThisYear = false,
            IsEveningOrWeekend = false,
            CampaignDiscountPercent = null
        };
    }
}