using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CustomerCommands;

namespace BookRight.Facade.Commands.CustomerCommands
{
    public interface IUpdateCustomer
    {
        Task HandleAsync(UpdateCustomerRequest command, CancellationToken cancellationToken = default);
    }
}
