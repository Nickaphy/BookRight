using BookRight.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.BookingCommand;
using BookRight.Facade.Commands.Booking;

namespace BookRight.Application.Commands.BookingCommands
{
    public class MarkAsNoShowCommandHandler : IMarkAsNoShowUseCase
    {
        private readonly IBookingRepository _bookingRepo;
        public MarkAsNoShowCommandHandler(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }
        public async Task ExecuteAsync(MarkAsNoShowRequest request)
        {

            var booking = await _bookingRepo.GetByIdAsync(request.BookingId)
                ?? throw new Exception($"Booking with ID {request.BookingId} not found.");
            booking.MarkNoShow(); // Kalder MarkNoShow metoden på booking entiteten for at ændre status til NoShow
            await _bookingRepo.SaveChangesAsync(); // Gemmer ændringerne i databasen


        }
    }
}
