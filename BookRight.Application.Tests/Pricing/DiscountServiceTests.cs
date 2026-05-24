using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.DiscountStrategy;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using Moq;

namespace BookRight.Application.Tests.Pricing;

public class DiscountServiceTests
{
    [Fact]
    public async Task GetBestDiscountAsync_ReturnsHighestDiscount()
    {
        // Arrange

        // Bronze strategy returns 50.
        var bronzeStrategy = new Mock<IDiscountStrategy>();

        bronzeStrategy
            .Setup(x => x.DiscountType)
            .Returns(DiscountType.Bronze);

        bronzeStrategy
            .Setup(x => x.CalculateDiscountAsync(It.IsAny<BookingPricingContext>()))
            .ReturnsAsync(50m);

        // Gold strategy returns 150.
        var goldStrategy = new Mock<IDiscountStrategy>();

        goldStrategy
            .Setup(x => x.DiscountType)
            .Returns(DiscountType.Gold);

        goldStrategy
            .Setup(x => x.CalculateDiscountAsync(It.IsAny<BookingPricingContext>()))
            .ReturnsAsync(150m);

        var strategies = new List<IDiscountStrategy>
        {
            bronzeStrategy.Object,
            goldStrategy.Object
        };

        var service = new DiscountService(strategies);

        var context = CreatePricingContext();

        // Act
        var result = await service.GetBestDiscountAsync(context);

        // Assert

        // Highest discount should win.
        Assert.Equal(150m, result.BestDiscount);

        // Gold strategy should be selected.
        Assert.Equal(
            DiscountType.Gold,
            result.WinningDiscountType);
    }

    [Fact]
    public async Task GetBestDiscountAsync_WhenAllStrategiesReturnZero_ReturnsNone()
    {
        // Arrange

        var strategy = new Mock<IDiscountStrategy>();

        strategy
            .Setup(x => x.DiscountType)
            .Returns(DiscountType.None);

        strategy
            .Setup(x => x.CalculateDiscountAsync(It.IsAny<BookingPricingContext>()))
            .ReturnsAsync(0m);

        var service = new DiscountService(
            new List<IDiscountStrategy>
            {
                strategy.Object
            });

        var context = CreatePricingContext();

        // Act
        var result = await service.GetBestDiscountAsync(context);

        // Assert
        Assert.Equal(0m, result.BestDiscount);

        Assert.Equal(
            DiscountType.None,
            result.WinningDiscountType);
    }

    private static BookingPricingContext CreatePricingContext()
    {
        // Shared helper used by all tests.
        var booking = Booking.Create(
            customerId: Guid.NewGuid(),
            practitionerId: Guid.NewGuid(),
            clinicId: Guid.NewGuid(),
            treatmentTypeId: Guid.NewGuid(),
            timeRange: new TimeRange(
                DateTime.Today.AddHours(10),
                DateTime.Today.AddHours(11)),
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