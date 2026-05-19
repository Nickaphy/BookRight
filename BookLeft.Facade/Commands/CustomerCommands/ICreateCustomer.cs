using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CustomerCommands;

namespace BookRight.Facade.Commands.CustomerCommands
{
    public interface ICreateCustomer
    {
        Task HandleAsync(CreateCustomerRequest command, CancellationToken cancellationToken = default);
    }
}
