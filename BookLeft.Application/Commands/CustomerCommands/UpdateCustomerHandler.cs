using BookRight.Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CustomerCommands;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Application.Repositories;

namespace BookRight.Application.Commands.CustomerCommands
{
    public class UpdateCustomerHandler : IUpdateCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task HandleAsync(UpdateCustomerRequest command, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(command.CustomerId, cancellationToken)
                ?? throw new InvalidOperationException("Customer not found");

            customer.UpdateName(command.Name);
            customer.UpdatePhoneNumber(command.PhoneNumber);
            customer.UpdateEmail(command.Email);
            customer.UpdateNote(command.Note);
            customer.UpdateStreet(command.Street);
            customer.UpdateCity(command.City);
            customer.UpdateZipcode(command.Zipcode);
            customer.updateDateOfBirth(command.DateOfBirth);

            await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
            await _customerRepository.SaveAsync(cancellationToken);
        }
    }
}
