// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Entities.Clinics;


// Eriks work

namespace BookRight.Application.Repositories;

public interface IClinicRepository
{
    Task<Clinic?> GetByIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken = default);
}