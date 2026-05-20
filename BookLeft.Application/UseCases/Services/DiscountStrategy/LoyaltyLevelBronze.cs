using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelBronze : IDiscountStrategy
    {
        private readonly decimal _percentage;
        private readonly ICustomerRepository _customerRepository;

        public LoyaltyLevelBronze(ICustomerRepository customerRepository, decimal percentage = 0.05m)
        {
            _percentage = percentage;
            _customerRepository = customerRepository;
        }

        public async Task<decimal> CalculateDiscount(Booking booking)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(booking.CustomerId);
            if (customer == null)
                return 0m;
            var loyaltyLevel = customer.LoyaltyLevel == LoyaltyLevel.Bronze;
            return loyaltyLevel ? booking.BasePrice.Amount * _percentage : 0;
            

        }
    }  
}
