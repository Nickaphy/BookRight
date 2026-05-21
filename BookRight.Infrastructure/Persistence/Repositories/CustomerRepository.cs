
using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Customers;
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
public class CustomerRepository : ICustomerRepository                                 //UDKOMMENTERET LUCAS d. 17.5 FEJL
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }


    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
     
    public async Task<bool> GetCustomerByPhoneNumberAsync(                        //UDKOMMENTERET LUCAS d. 17.5 FEJL
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
    }

    public async Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        
    }

    public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        
        _context.Customers.Remove(customer);
    }





}
