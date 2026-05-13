// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Bookings;
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

    Task<Booking?> GetByIdAsync(                                            //Lucas rettet - 13/5 - 13.55
        Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(
        Booking booking, CancellationToken cancellationToken = default);    //Lucas rettet - 13/5 - 13.55
}
