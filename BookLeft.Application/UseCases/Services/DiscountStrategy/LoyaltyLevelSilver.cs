using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelSilver : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public LoyaltyLevelSilver(decimal percentage = 0.10m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
            => booking.BasePrice.Amount * _percentage;
    }
}
