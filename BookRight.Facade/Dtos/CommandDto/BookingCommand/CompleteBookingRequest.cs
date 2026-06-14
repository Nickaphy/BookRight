using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.BookingCommand
{
    public record CompleteBookingRequest(Guid BookingId);
}
