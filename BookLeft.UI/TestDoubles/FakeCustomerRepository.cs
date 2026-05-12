// Erik's work

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;

namespace BookRight.UI.TestDoubles;

// Fake repository used to simulate customer lookup.
// Returns a valid test customer instead of querying a database.
public sealed class FakeCustomerRepository : ICustomerRepository
{
    public Task<Customer?> GetByIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = new Customer(
            name: "Test Customer",
            phoneNumber: "12345678",
            email: "test@example.com",
            loyaltyLevel: LoyaltyLevel.None,
            dateOfBirth: new DateTime(1990, 1, 1),
            note: "Fake customer used for UI flow testing.",
            street: "Test Street 1",
            city: "Test City",
            zipcode: "1234");

        return Task.FromResult<Customer?>(customer);
    }
}