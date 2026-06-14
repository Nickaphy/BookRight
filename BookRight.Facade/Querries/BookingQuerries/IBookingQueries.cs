using BookRight.Facade.Dtos.QuerryDto.BookingQuery;

namespace BookRight.Facade.Querries.BookingQuerries;

public interface IBookingQueries
{
    // Today's bookings — used by the auto-complete timer on the Bookinger page.
    Task<IReadOnlyList<BookingDto>> GetTodaysBookingsAsync();

    // Bookings for a given date range — used for the upcoming-month schedule.
    Task<IReadOnlyList<BookingDto>> GetBookingsForPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BookingDto>> GetUnhandledPastBookingsAsync(
    CancellationToken cancellationToken = default);
}