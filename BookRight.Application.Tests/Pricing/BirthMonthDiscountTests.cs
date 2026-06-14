using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.DiscountStrategy;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

// Erik's Work.

namespace BookRight.Application.Tests.Pricing;

public class BirthMonthDiscountTests
{
    [Fact]
    public async Task CalculateDiscountAsync_WhenBirthdayMonthAndNotUsed_Returns25PercentDiscount()
    {
        // Arrange: customer is eligible for birthday discount.
        var strategy = new BirthMonthDiscount();

        var context = CreatePricingContext(
            isBirthdayMonth: true,
            hasUsedBirthdayDiscountThisYear: false,
            basePrice: 1000m);

        // Act: calculate discount amount.
        var discount = strategy.CalculateDiscount(context);

        // Assert: 25% of 1000 = 250.
        Assert.Equal(250m, discount);
    }

    [Fact]
    public async Task CalculateDiscountAsync_WhenNotBirthdayMonth_ReturnsZero()
    {
        // Arrange: birthday discount should not apply.
        var strategy = new BirthMonthDiscount();

        var context = CreatePricingContext(
            isBirthdayMonth: false,
            hasUsedBirthdayDiscountThisYear: false,
            basePrice: 1000m);

        // Act
        var discount = strategy.CalculateDiscount(context);

        // Assert
        Assert.Equal(0m, discount);
    }

    [Fact]
    public async Task CalculateDiscountAsync_WhenAlreadyUsedThisYear_ReturnsZero()
    {
        // Arrange: customer has already used birthday discount this year.
        var strategy = new BirthMonthDiscount();

        var context = CreatePricingContext(
            isBirthdayMonth: true,
            hasUsedBirthdayDiscountThisYear: true,
            basePrice: 1000m);

        // Act
        var discount = strategy.CalculateDiscount(context);

        // Assert
        Assert.Equal(0m, discount);
    }

    [Theory]
    [InlineData(1000, 250)]
    [InlineData(800, 200)]
    [InlineData(400, 100)]
    [InlineData(1200, 300)]
    public async Task CalculateDiscountAsync_WhenEligible_Returns25PercentOfBasePrice(
        decimal basePrice,
        decimal expectedDiscount)
    {
        // Arrange:
        // Theory test verifies the same business rule
        // with different base prices.
        var strategy = new BirthMonthDiscount();

        var context = CreatePricingContext(
            isBirthdayMonth: true,
            hasUsedBirthdayDiscountThisYear: false,
            basePrice: basePrice);

        // Act
        var discount = strategy.CalculateDiscount(context);

        // Assert
        Assert.Equal(expectedDiscount, discount);
    }

    private static BookingPricingContext CreatePricingContext(
        bool isBirthdayMonth,
        bool hasUsedBirthdayDiscountThisYear,
        decimal basePrice)
    {
        // Helper keeps test setup simple and focused.
        var booking = Booking.Create(
            customerId: Guid.NewGuid(),
            practitionerId: Guid.NewGuid(),
            clinicId: Guid.NewGuid(),
            treatmentTypeId: Guid.NewGuid(),
            timeRange: new TimeRange(
                DateTime.Today.AddDays(1).AddHours(10),
                DateTime.Today.AddDays(1).AddHours(11)),
            basePrice: new Money(basePrice),
            isTeam: false);

        var customer = Customer.Create(
            name: "Test Customer",
            phoneNumber: "12345678",
            email: "test@test.dk",
            loyaltyLevel: LoyaltyLevel.None,
            dateOfBirth: new DateTime(1990, 5, 10),
            note: null,
            street: "Testvej 1",
            city: "Vejle",
            zipcode: "7100");

        return new BookingPricingContext
        {
            Booking = booking,
            Customer = customer,
            IsBirthdayMonth = isBirthdayMonth,
            HasUsedBirthdayDiscountThisYear = hasUsedBirthdayDiscountThisYear,
            IsEveningOrWeekend = false,
            CampaignDiscountPercent = null
        };
    }
}