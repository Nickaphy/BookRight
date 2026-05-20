using BookRight.Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CustomerCommands;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;

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
            var customer = await _customerRepository.GetCustomerByIdAsync(command.Id, cancellationToken)
            ?? throw new UseCaseException($"Customer with id {command.Id} was not found.");

            customer.Update(
                command.Name,
                command.PhoneNumber,
                command.Email,
                command.Street,
                command.City,
                command.Zipcode,
                command.DateOfBirth
            );

            await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);


        }
    }
}
