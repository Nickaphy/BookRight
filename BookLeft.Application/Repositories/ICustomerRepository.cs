// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code
using System.Threading.Tasks;
using BookRight.Domain.Customers;

namespace BookRight.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber);
    }
}
