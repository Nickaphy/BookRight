using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.BookingCommand
{
    public record CompleteBookingRequest(Guid BookingId);
}
