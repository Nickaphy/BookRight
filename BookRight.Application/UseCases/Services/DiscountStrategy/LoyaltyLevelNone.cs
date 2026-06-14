using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// No loyalty discount strategy.
//
// Rules:
// - Applies only to customers without loyalty status.
// - Returns zero discount.
public sealed class LoyaltyLevelNone : IDiscountStrategy
{

    // Identifies this strategy as no loyalty discount.
    public DiscountType DiscountType =>
        DiscountType.None;

    public decimal CalculateDiscount(
        BookingPricingContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // No discount is given for LoyaltyLevel.None.
        return 0m;
    }
}