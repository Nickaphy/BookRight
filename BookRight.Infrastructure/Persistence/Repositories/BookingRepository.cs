// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain



// Repository implementation using EF Core
//
// Responsibilities:
// - Query data using LINQ
// - Save data using DbContext
//
// Important:
// - Uses IQueryable for efficient queries
// - Avoids loading unnecessary data
// - Calls SaveChangesAsync for persistence
//
// This is where EF Core meets Application layer

// Erik's work

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _dbContext;

    public BookingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Checks whether the practitioner already has a booking
    // overlapping the requested time range.
    public async Task<bool> HasOverlappingBookingForPractitionerAsync(
        Guid practitionerId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .AnyAsync(
                booking =>
                booking.PractitionerId == practitionerId &&
                booking.TimeRange.Start < timeRange.End &&
                booking.TimeRange.End > timeRange.Start &&
                (booking.Status == BookingStatus.Created ||
                 booking.Status == BookingStatus.Completed), // Lucas ændret..
            cancellationToken);
    }

    // Checks whether the clinic has an overlapping booking.
    // MVP version: one overlapping booking means no available room.
    public async Task<bool> HasOverlappingBookingForClinicAsync(
        Guid clinicId,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        // Count how many active bookings overlap this time range.
        // Only Created and Completed bookings occupy a treatment room.
        // Cancelled and NoShow bookings free the room.
        var overlappingCount = await _dbContext.Bookings
            .CountAsync(
                booking =>
                    booking.ClinicId == clinicId &&
                    booking.TimeRange.Start < timeRange.End &&
                    booking.TimeRange.End > timeRange.Start &&
                    (booking.Status == BookingStatus.Created ||
                     booking.Status == BookingStatus.Completed),
                cancellationToken);

        // Compare against the clinic's number of treatment rooms.
        var clinic = await _dbContext.Clinics
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == clinicId, cancellationToken);

        var maxRooms = clinic?.AmountTreatmentRooms ?? 1;
        return overlappingCount >= maxRooms;
    }

    // Adds a new booking to EF Core change tracking.
    public async Task AddAsync(
        Booking booking,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    // Persists all tracked changes to SQL Server.
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _dbContext.Bookings.Update(booking);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetTotalSpentLastYearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        return await _dbContext.Bookings
            .Where(b => b.CustomerId == customerId
                   && b.CreatedDate >= oneYearAgo
                   && (b.Status == BookingStatus.Completed
                   || b.Status == BookingStatus.Created))
            .SumAsync(b => b.FinalPrice.Amount, cancellationToken);
    }

    // Checks whether the customer has already used
    // the birthday discount during the specified year.
    public async Task<bool> HasUsedBirthdayDiscountAsync(
        Guid customerId,
        int year,
        int birthMonth, //Lucas rettet
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings.AnyAsync(
            booking =>
                booking.CustomerId == customerId &&
                booking.TimeRange.Start.Year == year &&
                booking.TimeRange.Start.Month == birthMonth && //Lucas rettet
                booking.DiscountType == DiscountType.BirthdayMonth,
            cancellationToken);
    }

    public async Task<bool> HasOverlappingBookingForCustomerAsync(
      Guid customerId,
      TimeRange timeRange,
      CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .AnyAsync(
                booking =>
                    booking.CustomerId == customerId &&
                    booking.TimeRange.Start < timeRange.End &&
                    booking.TimeRange.End > timeRange.Start &&
                    (booking.Status == BookingStatus.Created ||
                     booking.Status == BookingStatus.Completed),
                cancellationToken);
    }

}