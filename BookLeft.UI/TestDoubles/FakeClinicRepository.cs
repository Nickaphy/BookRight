// Erik's work.


using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Clinics;

namespace BookRight.UI.TestDoubles;

// Fake repository used to simulate clinic lookup.
// Returns a valid test clinic instead of querying a database.
public sealed class FakeClinicRepository : IClinicRepository
{
    public Task<Clinic?> GetByIdAsync(
        Guid clinicId,
        CancellationToken cancellationToken = default)
    {
        var clinic = new Clinic(
            name: "Test Clinic",
            amountTreatmentRooms: 5,
            street: "Test Street 1",
            city: "Test City",
            zipcode: "1234");

        return Task.FromResult<Clinic?>(clinic);
    }
}