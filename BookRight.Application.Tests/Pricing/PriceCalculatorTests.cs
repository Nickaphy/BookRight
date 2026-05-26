using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using Moq;

namespace BookRight.Application.Tests.Pricing;

public class PriceCalculatorTests
{
    [Fact]
    public async Task CalculateFinalPriceAsync_ReturnsCorrectFinalPrice()
    {
        // Arrange

        // Mock discount service result.
        var discountService = new Mock<IDiscountService>();

        discountService
            .Setup(x => x.GetBestDiscountAsync(
                It.IsAny<BookingPricingContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BestDiscountResultBuilder()
                .WithDiscount(DiscountType.Gold, 150m)
                .Build());

        var calculator = new PriceCalculator(
            discountService.Object);

        var context = CreatePricingContext();

        // Act
        var result = await calculator
            .CalculateFinalPriceAsync(context);

        // Assert

        // 1000 - 150 = 850
        Assert.Equal(850m, result.FinalPrice.Amount);

        Assert.Equal(
            DiscountType.Gold,
            result.WinningDiscountType);
    }

    [Fact]
    public async Task CalculateFinalPriceAsync_WhenNoDiscount_ReturnsBasePrice()
    {
        // Arrange
        var discountService = new Mock<IDiscountService>();

        discountService
            .Setup(x => x.GetBestDiscountAsync(
                It.IsAny<BookingPricingContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BestDiscountResultBuilder()
                .WithDiscount(DiscountType.None, 0m)
                .Build());

        var calculator = new PriceCalculator(
            discountService.Object);

        var context = CreatePricingContext();

        // Act
        var result = await calculator
            .CalculateFinalPriceAsync(context);

        // Assert
        Assert.Equal(1000m, result.FinalPrice.Amount);

        Assert.Equal(
            DiscountType.None,
            result.WinningDiscountType);
    }

    private static BookingPricingContext CreatePricingContext()
    {
        // Shared helper keeps tests focused.
        var booking = Booking.Create(
            customerId: Guid.NewGuid(),
            practitionerId: Guid.NewGuid(),
            clinicId: Guid.NewGuid(),
            treatmentTypeId: Guid.NewGuid(),
            timeRange: new TimeRange(
                DateTime.Today.AddDays(1).AddHours(10),
                DateTime.Today.AddDays(1).AddHours(11)),
            basePrice: new Money(1000m));

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
            IsBirthdayMonth = false,
            HasUsedBirthdayDiscountThisYear = false,
            IsEveningOrWeekend = false,
            CampaignDiscountPercent = null
        };
    }
}