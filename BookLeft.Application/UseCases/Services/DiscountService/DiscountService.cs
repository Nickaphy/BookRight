using BookRight.Application.UseCases.Services.DiscountStrategy;

namespace BookRight.Application.UseCases.Services.DiscountService;

// Coordinates all registered discount strategies
// and selects the best available discount.
public class DiscountService : IDiscountService
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;

    public DiscountService(
        IEnumerable<IDiscountStrategy> strategies)
    {
        _strategies = strategies;
    }

    public async Task<BestDiscountResult>
        GetBestDiscountAsync(
            BookingPricingContext context,
            CancellationToken ct = default)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var result = new BestDiscountResult();

        // Evaluate all discount strategies in parallel.
        //
        // Example:
        // - Loyalty discount
        // - Birthday discount
        // - Campaign discount
        var tasks = _strategies.Select(async strategy =>
        {
            ct.ThrowIfCancellationRequested();

            var discount =
                await strategy.CalculateDiscountAsync(
                    context);

            result.OfferDiscount(
                strategy.DiscountType,
                discount);
        });

        await Task.WhenAll(tasks);

        return result;
    }
}