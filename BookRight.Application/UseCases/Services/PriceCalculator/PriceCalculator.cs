using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

namespace BookRight.Application.UseCases.Services.PriceCalculator;

// Responsible for calculating
// the final booking price after discounts.
public class PriceCalculator : IPriceCalculator
{
    private readonly IDiscountService _discountService;

    public PriceCalculator(
        IDiscountService discountService)
    {
        _discountService = discountService;
    }

    public async Task<(
        Money FinalPrice,
        DiscountType WinningDiscountType)>
        CalculateFinalPriceAsync(
            BookingPricingContext context,
            CancellationToken ct = default)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // Execute all discount strategies
        // and determine the winning discount.
        var discountResult =
            await _discountService.GetBestDiscountAsync(
                context,
                ct);

        // Final price = BasePrice - DiscountAmount
        var finalPrice = new Money(
            context.Booking.BasePrice.Amount -
            discountResult.BestDiscount);

        return (
            finalPrice,
            discountResult.WinningDiscountType);
    }
}