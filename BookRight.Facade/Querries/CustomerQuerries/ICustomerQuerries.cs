using BookRight.Facade.Dtos.QuerryDto.CustomerDtos;

namespace BookRight.Facade.Querries.CustomerQuerries
{
    public interface ICustomerQuerries
    {
        Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CustomerDto>> SearchCustomersAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<CustomerDetailDto?> GetCustomerDetailAsync(Guid customerId, CancellationToken cancellationToken = default);
    }
}