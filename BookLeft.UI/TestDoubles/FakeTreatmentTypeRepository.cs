// Erik's work.


using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Treatments;
using BookRight.Domain.Enums;

namespace BookRight.UI.TestDoubles;

// Fake repository used to simulate treatment lookup.
// Returns a valid treatment type instead of querying a database.
/*public sealed class FakeTreatmentTypeRepository : ITreatmentTypeRepository  //UDKOMMENTERET LUCAS d. 17.5
{
    public Task<TreatmentType?> GetByIdAsync(
        Guid treatmentTypeId,
        CancellationToken cancellationToken = default)
    {
        var treatmentType = new TreatmentType(
            name: "Physiotherapy",
            durationMinutes: 45,
            basePrice: 500,
            needsAuthorization: AuthorizationType.Physiotherapist,
            maxParticipants: 1);

        return Task.FromResult<TreatmentType?>(treatmentType);
    }
}*/