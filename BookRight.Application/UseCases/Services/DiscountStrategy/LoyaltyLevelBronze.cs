using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// Bronze loyalty discount strategy.
//
// Rules:
// - Applies only to Bronze customers.
// - Returns discount amount in currency.
public sealed class LoyaltyLevelBronze : IDiscountStrategy
{
    private readonly decimal _percentage;

    public LoyaltyLevelBronze(decimal percentage = 0.05m)
    {
        _percentage = percentage;
    }

    // Identifies this strategy as Bronze loyalty discount.
    public DiscountType DiscountType =>
        DiscountType.Bronze;

    public Task<decimal> CalculateDiscountAsync(
        BookingPricingContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // Discount only applies to Bronze customers.
        if (context.Customer.LoyaltyLevel != LoyaltyLevel.Bronze)
            return Task.FromResult(0m);

        // Calculate discount amount.
        var discount =
            context.Booking.BasePrice.Amount * _percentage;

        return Task.FromResult(discount);
    }
}