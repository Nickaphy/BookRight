using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelGold : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public LoyaltyLevelGold(decimal percentage = 0.15m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
             => booking.BasePrice.Amount * _percentage;
    }
}
