using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Facade.Dtos.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Commands.CustomerCommands
{
    public class CreateCustomerHandler : ICreateCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task HandleAsync(CreateCustomerRequest command, CancellationToken cancellationToken = default)
        {
            var customer = Customer.Create(
                name: command.Name,
                phoneNumber: command.PhoneNumber,
                email: command.Email,
                loyaltyLevel: (LoyaltyLevel)command.LoyaltyLevel,  
                dateOfBirth: command.DateOfBirth,
                note: command.Note,
                street: command.Street,
                city: command.City,
                zipcode: command.Zipcode);

            await _customerRepository.AddCustomerAsync(customer, cancellationToken);
            await _customerRepository.SaveAsync(cancellationToken);
        }
    }
}
