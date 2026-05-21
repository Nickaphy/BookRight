using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.PriceCalculator
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IDiscountService _discountService;

        public PriceCalculator(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<(Money FinalPrice, string? WinningStrategy)> CalculateFinalPriceAsync(
            Booking booking,
            CancellationToken ct = default)
        {
            var discountResult = await _discountService.GetBestDiscountAsync(booking, ct);
            var finalPrice = new Money(booking.BasePrice.Amount - discountResult.BestDiscount);

            return (finalPrice, discountResult.WinningStrategy);
        }
    }
}
