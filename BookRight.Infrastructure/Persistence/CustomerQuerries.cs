using BookRight.Application;
using BookRight.Domain.Entities.Customers;
using BookRight.Facade.Dtos.CustomerDtos;
using BookRight.Facade.Querries.CustomerQuerries;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence
{
    public class CustomerQuerries : ICustomerQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public CustomerQuerries(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }


        // Returns ALL customers as a read-only list.
        public async Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();
            return await context.Customers
            .AsNoTracking()
            .Select(c => new CustomerDto(
                c.Id,
                c.Name,
                c.PhoneNumber,
                (CustomerLoyaltyLevel)c.LoyaltyLevel))
            .ToListAsync(cancellationToken);
        }

        // Searches customers by a free-text term matched against name, phone, or e-mail.
        public async Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();
            return await context.Customers
                    .AsNoTracking()
                        .Where(c => c.Name.Contains(searchTerm)
                         || c.PhoneNumber.Contains(searchTerm)
                         || c.Email.Contains(searchTerm))
                .Select(c => new CustomerDto(
                        c.Id,
                        c.Name,
                        c.PhoneNumber,
                        (CustomerLoyaltyLevel)c.LoyaltyLevel))
                    .ToListAsync(cancellationToken);
        }
    }
}
