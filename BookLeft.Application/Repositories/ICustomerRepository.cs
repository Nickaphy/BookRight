// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Entities.Customers;


// Eriks work

namespace BookRight.Application.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);
}
