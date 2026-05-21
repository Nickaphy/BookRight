using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelSilver : IDiscountStrategy
    {
        private readonly decimal _percentage;
        private readonly ICustomerRepository _customerRepository;

        public LoyaltyLevelSilver(ICustomerRepository customerRepository, decimal percentage = 0.10m)
        {
            _percentage = percentage;
            _customerRepository = customerRepository;
        }

        public async Task<decimal> CalculateDiscount(Booking booking)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(booking.CustomerId);
            if (customer == null)
                return 0m;
            var loyaltyLevel = customer.LoyaltyLevel == LoyaltyLevel.Silver;
            return loyaltyLevel ? booking.BasePrice.Amount * _percentage : 0;
        }
    }
}
