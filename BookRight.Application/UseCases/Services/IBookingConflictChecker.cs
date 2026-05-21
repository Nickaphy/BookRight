// Service abstraction for detecting booking conflicts
// Used by booking use cases before creating a booking


using BookRight.Domain.ValueObjects;

namespace BookRight.Application.Services;

// Service abstraction for booking conflict validation.
// Used by booking use cases before creating bookings.
public interface IBookingConflictChecker
{
    Task EnsurePractitionerAvailabilityAsync(
        Guid practitionerId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default);

    Task EnsureClinicAvailabilityAsync(
        Guid clinicId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default);
}
