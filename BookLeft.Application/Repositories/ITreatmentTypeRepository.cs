// Repository interface for treatment types.
// Used by Application layer to fetch treatment data without knowing EF Core.
// Implemented later in Infrastructure.

using BookRight.Domain.Entities.Treatments;


// Eriks work

namespace BookRight.Application.Repositories;

public interface ITreatmentTypeRepository
{
    Task<TreatmentType?> GetByIdAsync(
        Guid treatmentTypeId,
        CancellationToken cancellationToken = default);
}
