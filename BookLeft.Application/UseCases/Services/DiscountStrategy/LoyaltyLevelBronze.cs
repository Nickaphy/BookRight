using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelBronze : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public LoyaltyLevelBronze(decimal percentage = 0.05m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
            => booking.BasePrice.Amount * _percentage;
    }
}
