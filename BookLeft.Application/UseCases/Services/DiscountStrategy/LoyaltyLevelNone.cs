using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelNone : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public LoyaltyLevelNone(decimal percentage = 0.0m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
            => booking.BasePrice.Amount * _percentage;
    }
}
