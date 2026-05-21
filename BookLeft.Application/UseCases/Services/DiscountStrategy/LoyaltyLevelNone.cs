using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class LoyaltyLevelNone : IDiscountStrategy
    {
        private readonly decimal _percentage;
        private readonly ICustomerRepository _customerRepository;

        public LoyaltyLevelNone(ICustomerRepository customerRepisitory, decimal percentage = 0m)
        {
            _percentage = percentage;
            _customerRepository = customerRepisitory;
        }

        public async Task<decimal> CalculateDiscount(Booking booking)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(booking.CustomerId);
            if (customer == null)
                return 0m;
            var isNoneLoyalty = customer.LoyaltyLevel == LoyaltyLevel.None;
            return isNoneLoyalty ? booking.BasePrice.Amount * _percentage : 0;


        }
    }
}
