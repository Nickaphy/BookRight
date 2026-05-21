using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// Silver loyalty discount strategy.
//
// Rules:
// - Applies only to Silver customers.
// - Returns discount amount in currency.
public sealed class LoyaltyLevelSilver : IDiscountStrategy
{
    private readonly decimal _percentage;

    public LoyaltyLevelSilver(decimal percentage = 0.10m)
    {
        _percentage = percentage;
    }

    // Identifies this strategy as Silver loyalty discount.
    public DiscountType DiscountType =>
        DiscountType.Silver;

    public Task<decimal> CalculateDiscountAsync(
        BookingPricingContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // Discount only applies to Silver customers.
        if (context.Customer.LoyaltyLevel != LoyaltyLevel.Silver)
            return Task.FromResult(0m);

        // Calculate discount amount.
        var discount =
            context.Booking.BasePrice.Amount * _percentage;

        return Task.FromResult(discount);
    }
}