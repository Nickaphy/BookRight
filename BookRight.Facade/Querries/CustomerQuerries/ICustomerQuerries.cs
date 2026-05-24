using BookRight.Facade.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Querries.CustomerQuerries
{
    public interface ICustomerQuerries
    {

        Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
