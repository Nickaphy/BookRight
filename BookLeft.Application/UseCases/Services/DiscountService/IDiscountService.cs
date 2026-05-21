using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountService
{
    public interface IDiscountService
    {
        Task<BestDiscountResult> GetBestDiscountAsync(Booking booking, CancellationToken ct = default);
    }
}
