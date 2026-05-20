using BookRight.Domain.Entities.Bookings;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Domain.ValueObjects;
using BookRight.Application.Repositories;

namespace BookRight.Application.UseCases.Services.DiscountStrategy
{
    public class BirthMonthDiscount : IDiscountStrategy
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly decimal _percentage;

        public BirthMonthDiscount(ICustomerRepository customerRepository, decimal percentage = 0.25m)
        {
            _customerRepository = customerRepository;
            _percentage = percentage;
        }

        public async Task<decimal> CalculateDiscount(Booking booking)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(booking.CustomerId);
            if (customer == null)
                return 0m;
            var isBirthMonth = customer.DateOfBirth.Month == DateTime.UtcNow.Month;

            return isBirthMonth ? booking.BasePrice.Amount * _percentage : 0m;  //bool der tjekker om det er fødselsdags måneden ellers returnerer den 0
            
        }
    }
}
