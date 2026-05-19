using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Domain.ValueObjects;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class BirthMonthDiscount : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public BirthMonthDiscount(decimal percentage = 0.25m)
        {
            _percentage = percentage;
        }

        public decimal CalculateDiscount(Booking booking)
            => booking.BasePrice.Amount * _percentage;
    }
}
