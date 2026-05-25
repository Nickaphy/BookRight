using BookRight.Facade.Dtos.BookingQuery;

namespace BookRight.Facade.Queries.BookingQueries;

public interface IBookingQueries
{
    Task<IReadOnlyList<BookingDto>> GetTodaysBookingsAsync();
}
