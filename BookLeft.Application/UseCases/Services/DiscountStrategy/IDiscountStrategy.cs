using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public interface IDiscountStrategy
    {
        // Calculate the discount for a given booking
        // Returns the discount amount (e.g., 0.10 for 10% off)
        decimal CalculateDiscount(Booking booking);
    }
}
