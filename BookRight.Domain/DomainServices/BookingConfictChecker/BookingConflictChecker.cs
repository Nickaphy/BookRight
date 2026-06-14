using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Enums;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Domain.DomainServices.BookingConfictChecker
{
    public class BookingConflictChecker : IBookingConflictChecker
    {
        public void ValidateWithoutOverlapping(
            TimeRange timeRange,
            int maxParticipants,
            IEnumerable<Booking> existingForCustomer,
            IEnumerable<Booking> existingForPractitioner)
        {
            var overlappingCount = existingForPractitioner
                .Count(b => b.Status == BookingStatus.Created &&
                b.TimeRange.Overlaps(timeRange));

            if (overlappingCount >= maxParticipants)
                throw new DomainException("This time slot is fully booked.");

            if (existingForCustomer.Any(p => p.Status == BookingStatus.Created && p.Status == BookingStatus.Completed && p.TimeRange.Overlaps(timeRange)))
                throw new DomainException("Customer is already booked for this time slot.");


        }
    }
}
