
using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Customers;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookRight.Infrastructure.Persistence.Repositories;


public class CustomerRepository : ICustomerRepository
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
    public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }

    public Task UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        return Task.CompletedTask;
    }

    public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

        if (customer == null)
            return;

        _context.Customers.Remove(customer);
    }
}