using BookRight.Application.UseCases.Services.DiscountStrategy;
using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;

        public DiscountService(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<decimal> GetBestDiscountAsync(
            Booking booking,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(booking);

            var tasks = _strategies.Select(s => Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();
                return s.CalculateDiscount(booking);
            }, ct))
            .ToArray();

            var discounts = await Task.WhenAll(tasks);
            return discounts.Max();
        }
    }
}
