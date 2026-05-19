using BookRight.Application.Repositories;
using BookRight.Facade.Commands.CustomerCommands;
using BookRight.Facade.Dtos.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Commands.CustomerCommands
{
    public class DeleteCustomerHandler : IDeleteCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task HandleAsync(DeleteCustomerRequest request, CancellationToken cancellationToken = default)
        {
            var customerAlreadyExists = await _customerRepository.GetCustomerByIdAsync(request.Id, cancellationToken);
            if (customerAlreadyExists == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            // Mark the customer as deleted in the repository
            await _customerRepository.DeleteCustomerAsync(request.Id, cancellationToken);
            await _customerRepository.SaveAsync(cancellationToken);

        }
    }
}
