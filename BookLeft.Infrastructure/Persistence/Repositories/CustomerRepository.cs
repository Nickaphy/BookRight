using Bookright.Domain.Entities.Customers;
using BookRight.Application.Repositories;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookRight.Infrastructure.Persistence.Repositories;


// This class translates the abstract repository contract into concrete
// LINQ-to-SQL operations using the AppDbContext.
// It implements the ICustomerRepository interface defined in the Application layer,
// so that the dependencies only point inwards (Clean Architecture/Onion).
// Infrastructure layer can know about EF Core and database details,
// but Application can't and shall be focused on business logic.
public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }


    // -----
    // Query Implementations
    // -----

    // FindAsync is preferred over FirstOrDefaultAsync(x => x.CustomerId == id)
    // when looking up by primary key, because:
    //   1. EF Core checks its in-memory identity map FIRST — if the entity was
    //      already loaded in this request/context lifetime, no SQL is issued.
    //   2. It generates a simple "WHERE Id = @p0" query when it does hit the DB.

    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public async Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        // ToListAsync materialises the query into a List<Customer> in memory.
        // We cast to IReadOnlyList to prevent callers from accidentally
        // mutating the collection (following the principle of least surprise).
        //
        // AsNoTracking() is used here because this is a READ-ONLY query:
        //   - No changes will be made to these entities.
        //   - EF Core will skip building its change-tracking snapshots.
        //   - Result: faster query, lower memory usage.
        //
        // Only skip AsNoTracking when you intend to update the entity later
        // in the SAME DbContext lifetime — e.g. GetByIdAsync for an edit use case.
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Array.Empty<Customer>();

        var term = searchTerm.ToLowerInvariant();


        // We search across: full name, e-mail, and phone number — matching the
        // receptionist's workflow (they may know a customer by name OR phone).
        return await _context.Customers
            .AsNoTracking()
            .Where(c =>
                c.Name.ToLower().Contains(term) ||
                c.Email.ToLower().Contains(term) ||
                c.PhoneNumber.Contains(term))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> GetCustomerByPhoneNumberAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default)
    {
        // AnyAsync generates a SQL EXISTS query — far more efficient than loading
        // the full Customer row just to check presence.
        // SQL: SELECT CASE WHEN EXISTS (SELECT 1 FROM Customers WHERE ...) THEN 1 ELSE 0 END

        var normalised = phoneNumber;

        return await _context.Customers
            .AnyAsync(
                c => c.PhoneNumber == normalised,
                cancellationToken);
    }

    // -----
    // Command Implementations
    // -----

    public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);

        // SaveChangesAsync must still be called by the Use Case.
    }

    public Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        // If the customer was loaded via GetByIdAsync earlier IN THE SAME DbContext
        // lifetime (same HTTP request in Blazor Server), EF Core is already tracking
        // it. Calling Update() re-attaches a detached entity or resets its state to
        // "Modified", which causes EF to generate an UPDATE for ALL columns.

        _context.Customers.Update(customer);
        return Task.CompletedTask;

        // SaveChangesAsync must still be called by the Use Case.
    }

    //Overvej om denne skal være Customer customer i stedet for guid Id. Skal matche med ICustomerRepository.cs.
    public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Find the entity first so we can hand it to Remove().
        // FindAsync checks the identity map before hitting the database,
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

        // Guard: if the customer doesn't exist, silently succeed.
        // Alternatively you could throw a domain exception — document your
        // choice in the Architecture Decision Record.
        if (customer == null)
            return;

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.FindAsync(id);
        }
        // Remove() marks the entity as "Deleted". EF Core will generate a
        // DELETE statement when SaveChangesAsync() is called.
        _context.Customers.Remove(customer);

        // SaveChangesAsync must be called by the Use Case.
    }
}