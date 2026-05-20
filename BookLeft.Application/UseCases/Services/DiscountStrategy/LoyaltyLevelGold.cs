using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelGold : IDiscountStrategy
    {
        private readonly decimal _percentage;
        private readonly ICustomerRepository _customerRepository;

        public LoyaltyLevelGold(ICustomerRepository customerRepository, decimal percentage = 0.15m)
        {
            _percentage = percentage;
            _customerRepository = customerRepository;
        }

        public async Task<decimal> CalculateDiscount(Booking booking)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(booking.CustomerId);
            if (customer == null)
                return 0m;
            var isGoldLoyalty = customer.LoyaltyLevel == LoyaltyLevel.Gold;
            return isGoldLoyalty ? booking.BasePrice.Amount * _percentage  : 0;


        }
    }
}
