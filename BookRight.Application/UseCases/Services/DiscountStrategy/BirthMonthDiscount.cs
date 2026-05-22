using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountStrategy;

// Birthday month discount strategy.
//
// Rules:
// - Customer receives birthday discount
//   during birth month.
// - Discount may only be used once per year.
// - Returns discount amount in currency.
public sealed class BirthMonthDiscount : IDiscountStrategy
{
    private readonly decimal _percentage;

    public BirthMonthDiscount(decimal percentage = 0.25m)
    {
        _percentage = percentage;
    }

    // Identifies this strategy as birthday discount.
    public DiscountType DiscountType =>
        DiscountType.BirthdayMonth;

    public Task<decimal> CalculateDiscountAsync(
        BookingPricingContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        // Customer is not in birthday month.
        if (!context.IsBirthdayMonth)
            return Task.FromResult(0m);

        // Customer already used birthday discount this year.
        if (context.HasUsedBirthdayDiscountThisYear)
            return Task.FromResult(0m);

        // Calculate discount amount.
        var discount =
            context.Booking.BasePrice.Amount * _percentage;

        return Task.FromResult(discount);
    }
}