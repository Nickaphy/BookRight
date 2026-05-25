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
        var today = DateTime.Today;
        return await GetBookingsForPeriodAsync(today, today.AddDays(1));
    }

    public async Task<IReadOnlyList<BookingDto>> GetBookingsForPeriodAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        using var context = _factory.CreateDbContext();

        var rows = await context.Bookings
            .AsNoTracking()
            .Where(b => b.TimeRange.Start >= from && b.TimeRange.Start < to)
            .Join(context.Customers,
                b => b.CustomerId,
                c => c.Id,
                (b, c) => new { Booking = b, CustomerName = c.Name })
            .Join(context.Treatments,
                x => x.Booking.TreatmentTypeId,
                t => t.Id,
                (x, t) => new { x.Booking, x.CustomerName, TreatmentName = t.Name })
            .Join(context.Clinics,
                x => x.Booking.ClinicId,
                cl => cl.Id,
                (x, cl) => new
                {
                    x.Booking.Id,
                    x.CustomerName,
                    x.TreatmentName,
                    x.Booking.TimeRange.Start,
                    x.Booking.TimeRange.End,
                    x.Booking.Status,
                    ClinicId = cl.Id,
                    ClinicName = cl.Name
                })
            .ToListAsync(cancellationToken);

        return rows
            .OrderBy(r => r.Start)
            .Select(r => new BookingDto(
                r.Id,
                r.CustomerName,
                r.TreatmentName,
                r.Start,
                r.End,
                r.Status.ToString(),
                r.ClinicId,
                r.ClinicName))
            .ToList();
    }
}