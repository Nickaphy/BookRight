using BookRight.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Domain.Entities.Bookings
{
    public sealed record BookingCompletedEvent(
    Guid CustomerId,
    decimal AmountPaid) : IDomainEvent;
}
