// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;



// Eriks work
namespace BookRight.Application.Repositories;

public interface IBookingRepository
{
    Task<bool> HasOverlappingBookingForPractitionerAsync
            (
        Guid practiotionerId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default
        );

    Task<bool> HasOverlappingBookingForClinicAsync(
        Guid clinicId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
