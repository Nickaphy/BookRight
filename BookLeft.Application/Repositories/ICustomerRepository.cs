// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code


using BookRight.Domain.Entities.Customers;

namespace BookRight.Application.Repositories;

public interface ICustomerRepository
{
    // -----
    // Search/fetching operations
    // -----

    Task SaveAsync(CancellationToken cancellationToken = default);
    // Retrieves a single customer by their unique identifier.
    // Returns null if no customer with the given ID exists.
    Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    // Retrieves a single customer by their phone number.
    


    // -----
    // CRUD operations
    // -----

    // Persists a brand-new Customer aggregate to the data store.

    // The Use Case is responsible for constructing the Customer via its factory
    // method / constructor before calling this. The repository does not create
    // domain objects — it only stores them. This keeps domain logic out of
    // infrastructure code.
    Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default);

    // Marks an existing Customer aggregate as modified so EF Core will generate
    // an UPDATE statement on the next SaveChangesAsync.
    Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);

    // Soft-deletes or hard - deletes a customer record.
    //Jeg er ikke sikker på denne, da den foreslår at det skal være Customer customer i stedet for ID.
    //Skal dog matche med CustomerRepository.cs implementationen.
    Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);
}
