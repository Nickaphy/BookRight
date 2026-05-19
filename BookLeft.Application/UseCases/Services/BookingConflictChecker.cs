using BookRight.Application.Repositories;
using BookRight.Domain.ValueObjects;

namespace BookRight.Application.Services;

// Coordinates booking conflict validation
// across repositories and scheduling rules.
public sealed class BookingConflictChecker : IBookingConflictChecker
{
    private readonly IBookingRepository _bookingRepository;

    public BookingConflictChecker(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task EnsurePractitionerAvailabilityAsync(
        Guid practitionerId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        var hasOverlap =
            await _bookingRepository
                .HasOverlappingBookingForPractitionerAsync(
                    practitionerId,
                    timeRange,
                    cancellationToken);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "The practitioner already has a booking in this time range.");
        }
    }

    public async Task EnsureClinicAvailabilityAsync(
        Guid clinicId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        var hasOverlap =
            await _bookingRepository
                .HasOverlappingBookingForClinicAsync(
                    clinicId,
                    timeRange,
                    cancellationToken);

        if (hasOverlap)
        {
            throw new InvalidOperationException(
                "The clinic has no available room in this time range.");
        }
    }
}