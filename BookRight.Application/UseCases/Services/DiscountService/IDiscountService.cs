using BookRight.Domain.ValueObjects;

namespace BookRight.Application.UseCases.Services.DiscountService;

// Coordinates all discount strategies
// and selects the best available discount.
public interface IDiscountService
{
    Task<BestDiscountResult> GetBestDiscountAsync(
        BookingPricingContext context,
        CancellationToken ct = default);
}