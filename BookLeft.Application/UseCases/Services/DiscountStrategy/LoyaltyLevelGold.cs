using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// Gold loyalty discount strategy.
//
// Rules:
// - Applies only to Gold customers.
// - Returns discount amount in currency.
public sealed class LoyaltyLevelGold : IDiscountStrategy
{
    private readonly decimal _percentage;

    public LoyaltyLevelGold(decimal percentage = 0.15m)
    {
        _percentage = percentage;
    }

    // Identifies this strategy as Gold loyalty discount.
    public DiscountType DiscountType =>
        DiscountType.Gold;

    public Task<decimal> CalculateDiscountAsync(
        BookingPricingContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // Discount only applies to Gold customers.
        if (context.Customer.LoyaltyLevel != LoyaltyLevel.Gold)
            return Task.FromResult(0m);

        // Calculate discount amount.
        var discount =
            context.Booking.BasePrice.Amount * _percentage;

        return Task.FromResult(discount);
    }
}