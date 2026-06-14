using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Domain.DomainServices.BookingConfictChecker
{
    public interface IBookingConflictChecker
    {
        void ValidateWithoutOverlapping(
        TimeRange timeRange,
        int maxParticipants,
        IEnumerable<Booking> existingForCustomer,
        IEnumerable<Booking> existingForPractitioner);
    }
}
