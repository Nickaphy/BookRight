// Erik's work.

// Fake repositories are temporary test doubles.
// They allow us to test the application flow before implementing EF Core persistence.

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;

namespace BookRight.UI.TestDoubles;

// Fake repository used for early UI/Application testing.
// Simulates database behavior without EF Core or SQL Server.
public sealed class FakeBookingRepository : IBookingRepository
{
    // Always returns false to simulate no overlap conflicts.
    public Task<bool> HasOverlappingBookingForPractitionerAsync(
        Guid practitionerId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    // Always returns false to simulate available clinic capacity.
    public Task<bool> HasOverlappingBookingForClinicAsync(
        Guid clinicId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    // Simulates adding a booking to persistence.
    public Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    // Simulates saving changes to a database.
    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}