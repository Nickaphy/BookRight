// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code
public interface IPractitionerRepository
{
    Task<Practitioner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Practitioner practitioner, CancellationToken cancellationToken = default);
    Task UpdateAsync(Practitioner practitioner, CancellationToken cancellationToken = default);
    Task DeleteAsync(Practitioner practitioner, CancellationToken cancellationToken = default);
}