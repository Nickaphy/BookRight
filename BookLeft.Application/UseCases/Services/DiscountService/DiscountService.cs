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

        public async Task<BestDiscountResult> GetBestDiscountAsync(Booking booking, 
            CancellationToken ct = default)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking));

            var result = new BestDiscountResult();

            var tasks = _strategies.Select(s => Task.Run(async () =>
            {
                ct.ThrowIfCancellationRequested();
                var discount = await s.CalculateDiscount(booking);
                var strategyName = s.GetType().Name;
                result.OfferDiscount(strategyName, discount);
            }, ct))
            .ToArray();

            await Task.WhenAll(tasks);
            return result;
        }
    }
}
