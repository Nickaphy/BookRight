using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CommandDto.CustomerCommands;

namespace BookRight.Facade.Commands.CustomerCommands
{
    public interface IDeleteCustomer
    {
        Task HandleAsync(DeleteCustomerRequest request, CancellationToken cancellationToken = default);
    }
}
