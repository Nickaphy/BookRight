using BookRight.Facade.Dtos.BookingQuery;
using BookRight.Facade.Queries.BookingQueries;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence;

public class BookingQueries : IBookingQueries
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public BookingQueries(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<BookingDto>> GetTodaysBookingsAsync()
    {
        using var context = _factory.CreateDbContext();
        var todayStart = DateTime.Today;
        var todayEnd = todayStart.AddDays(1);

        var rows = await context.Bookings
            .AsNoTracking()
            .Where(b => b.TimeRange.Start >= todayStart && b.TimeRange.Start < todayEnd)
            .Join(context.Customers,
                b => b.CustomerId,
                c => c.Id,
                (b, c) => new { Booking = b, CustomerName = c.Name })
            .Join(context.Treatments,
                x => x.Booking.TreatmentTypeId,
                t => t.Id,
                (x, t) => new
                {
                    x.Booking.Id,
                    x.CustomerName,
                    TreatmentName = t.Name,
                    x.Booking.TimeRange.Start,
                    x.Booking.TimeRange.End,
                    x.Booking.Status
                })
            .ToListAsync();

        return rows
            .OrderBy(r => r.Start)
            .Select(r => new BookingDto(
                r.Id,
                r.CustomerName,
                r.TreatmentName,
                r.Start,
                r.End,
                r.Status.ToString()))
            .ToList();
    }
}
