using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

namespace BookRight.Application.UseCases.Services.PriceCalculator;

// Calculates final booking prices
// after all discounts are evaluated.
public interface IPriceCalculator
{
    Task<(
        Money FinalPrice,
        DiscountType WinningDiscountType)>
        CalculateFinalPriceAsync(
            BookingPricingContext context,
            CancellationToken ct = default);
}