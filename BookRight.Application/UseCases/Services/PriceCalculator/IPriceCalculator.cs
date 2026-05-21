using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.PriceCalculator
{
    public interface IPriceCalculator
    {
        Task<(Money FinalPrice, string? WinningStrategy)> CalculateFinalPriceAsync(
            Booking booking,
            CancellationToken ct = default);
    }
}
