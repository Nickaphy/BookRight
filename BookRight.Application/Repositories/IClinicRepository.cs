// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Entities.Clinics;

namespace BookRight.Application.Repositories;

public interface IClinicRepository
{   
    Task AddAsync(Clinic clinic, CancellationToken cancellationToken = default);

    Task<Clinic?> GetByIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken = default);

    
    Task UpdateAsync(Clinic clinic, CancellationToken cancellationToken = default);

    Task DeleteAsync(Clinic clinic, CancellationToken cancellationToken = default);

    Task SaveAsync(Clinic clinic, CancellationToken cancellationToken = default);
}
