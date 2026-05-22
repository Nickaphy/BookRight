using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// Strategy abstraction for all discount policies.
//
// Each strategy:
// - evaluates whether a discount applies
// - calculates discount amount
// - exposes its DiscountType
//
// Strategies should ONLY contain
// business policy logic.
public interface IDiscountStrategy
{
    // Identifies which discount type
    // this strategy represents.
    DiscountType DiscountType { get; }

    // Calculates discount amount.
    //
    // Returns:
    // - 0 when discount does not apply
    // - discount amount in currency otherwise
    Task<decimal> CalculateDiscountAsync(
        BookingPricingContext context);
}