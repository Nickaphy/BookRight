using BookRight.Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application
{
    public interface ICustomerQuerries
    {
        

        // Returns ALL customers as a read-only list.
        Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default);

        // Searches customers by a free-text term matched against name, phone, or e-mail.
        Task<IReadOnlyList<Customer>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
