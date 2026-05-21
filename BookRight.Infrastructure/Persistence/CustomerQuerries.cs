using BookRight.Application;
using BookRight.Domain.Entities.Customers;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence
{
    public class CustomerQuerries : ICustomerQuerries
    {
        private readonly AppDbContext _context;

        public CustomerQuerries(AppDbContext context)
        {
            _context = context;
        }
        

        // Returns ALL customers as a read-only list.
        public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
        }

        // Searches customers by a free-text term matched against name, phone, or e-mail.
        public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                    .AsNoTracking()
                    .Where(c => c.Name.Contains(searchTerm)
                             || c.PhoneNumber.Contains(searchTerm)
                             || c.Email.Contains(searchTerm))
                    .ToListAsync(cancellationToken);
        } 
    }
}
