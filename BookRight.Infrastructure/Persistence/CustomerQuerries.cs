using BookRight.Domain.Enums;
using BookRight.Facade.Dtos.CustomerDtos;
using BookRight.Facade.Querries.CustomerQuerries;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence
{
    public class CustomerQuerries : ICustomerQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public CustomerQuerries(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync(
            CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();
            var customers = await context.Customers
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return customers
                .Select(c => new CustomerDto(c.Id, c.Name, c.PhoneNumber, CustomerLoyaltyLevel.None, c.DateOfBirth))
                .ToList();
        }

        // Searches customers by name, phone number or e-mail.
        // The loyalty level in the returned DTO is calculated live from the
        // customer's booking history for the past 12 months — not read from the
        // stored column — so it always reflects the current reality.
        public async Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(
            string searchTerm,
            CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();

            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            var customers = await context.Customers
                .AsNoTracking()
                .Where(c => c.Name.Contains(searchTerm)
                         || c.PhoneNumber.Contains(searchTerm)
)
                .ToListAsync(cancellationToken);

            if (customers.Count == 0)
                return [];

            var customerIds = customers.Select(c => c.Id).ToList();

            // Sum FinalPrice per customer for the past 12 months.
            // Matches the logic in BookingRepository.GetTotalSpentLastYearAsync
            // and Customer.UpdateLoyaltyLevel so the displayed level is consistent.
            var spendingByCustomer = await context.Bookings
                .AsNoTracking()
                .Where(b => customerIds.Contains(b.CustomerId)
                         && b.CreatedDate >= oneYearAgo
                         && b.Status == BookingStatus.Completed)
                .GroupBy(b => b.CustomerId)
                .Select(g => new { CustomerId = g.Key, Total = g.Sum(b => b.FinalPrice.Amount) })
                .ToDictionaryAsync(x => x.CustomerId, x => x.Total, cancellationToken);

            return customers.Select(c =>
            {
                var total = spendingByCustomer.TryGetValue(c.Id, out var t) ? t : 0m;

                // Mirror the thresholds from Customer.UpdateLoyaltyLevel
                var level = total switch
                {
                    > 25000m => CustomerLoyaltyLevel.Gold,
                    > 10000m => CustomerLoyaltyLevel.Silver,
                    >= 3000m => CustomerLoyaltyLevel.Bronze,
                    _ => CustomerLoyaltyLevel.None
                };

                return new CustomerDto(c.Id, c.Name, c.PhoneNumber, level, c.DateOfBirth);
            }).ToList();
        }

        public async Task<CustomerDetailDto?> GetCustomerDetailAsync(
            Guid customerId,
            CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();

            var customer = await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

            if (customer is null) return null;

            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            var totalSpent = await context.Bookings
                .AsNoTracking()
                .Where(b => b.CustomerId == customerId
                         && b.CreatedDate >= oneYearAgo
                         && b.Status == BookingStatus.Completed)
                .SumAsync(b => (decimal?)b.FinalPrice.Amount ?? 0, cancellationToken);

            var level = totalSpent switch
            {
                >= 10000m => CustomerLoyaltyLevel.Gold,
                >= 5000m => CustomerLoyaltyLevel.Silver,
                >= 1000m => CustomerLoyaltyLevel.Bronze,
                _ => CustomerLoyaltyLevel.None
            };

            return new CustomerDetailDto(
                customer.Id,
                customer.Name,
                customer.PhoneNumber,
                customer.Email,
                customer.DateOfBirth,
                customer.Street,
                customer.City,
                customer.Zipcode,
                customer.Note,
                level,
                totalSpent);
        }
    }
}