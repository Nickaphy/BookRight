// Erik's work.

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Enums;

namespace BookRight.UI.TestDoubles;

// Fake repository used to simulate practitioner lookup.
// Returns a valid test practitioner instead of querying a database.
/*public sealed class FakePractitionerRepository : IPractitionerRepository  //UDKOMMENTERET LUCAS d. 17.5
{
    public Task<Practitioner?> GetByIdAsync(
        Guid practitionerId,
        CancellationToken cancellationToken = default)
    {
        var practitioner = new Practitioner(
            name: "Test Practitioner",
            email: "practitioner@example.com",
            phoneNumber: "12345678",
            authorizationCode: "AUTH-001",
            authorizationType: AuthorizationType.Physiotherapist,
            clinicId: Guid.NewGuid());

        return Task.FromResult<Practitioner?>(practitioner);
    }
}*/