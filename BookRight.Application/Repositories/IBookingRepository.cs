// Repository interface
// Defines what the Application layer needs from persistence
// Implemented later in Infrastructure using EF Core
// Application depends on abstraction, not concrete database code

using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;

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

    Task<Booking?> GetByIdAsync(                                            
        Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(
        Booking booking, CancellationToken cancellationToken = default);    

    Task<decimal> GetTotalSpentLastYearAsync(Guid customerId, CancellationToken cancellationToken = default);

    // Checks whether the customer has already used
    // the birthday discount during the specified year.
    Task<bool> HasUsedBirthdayDiscountAsync(
        Guid customerId,
        int year,
        int birthMonth,
        CancellationToken cancellationToken = default);

    Task<bool> HasOverlappingBookingForCustomerAsync(
      Guid customerId,
      TimeRange timeRange,
      CancellationToken cancellationToken = default);
}
