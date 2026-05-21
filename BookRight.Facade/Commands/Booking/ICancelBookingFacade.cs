using BookRight.Facade.Dtos.BookingCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Booking
{
    public interface ICancelBookingFacade
    {
        Task ExecuteAsync(CancelBookingRequest request);
    }
}
