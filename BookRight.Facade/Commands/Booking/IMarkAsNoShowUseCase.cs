using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;

namespace BookRight.Facade.Commands.Booking
{
    
        public interface IMarkAsNoShowUseCase
        {
            Task ExecuteAsync(MarkAsNoShowRequest request);
        }
    
}
