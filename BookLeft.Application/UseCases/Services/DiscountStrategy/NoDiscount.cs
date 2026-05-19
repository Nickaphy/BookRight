using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class NoDiscount : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public NoDiscount(decimal percentage = 0m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
            => booking.BasePrice.Amount * _percentage;

    }
}
