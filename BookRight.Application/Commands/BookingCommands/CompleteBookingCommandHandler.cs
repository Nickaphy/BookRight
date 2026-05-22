using BookRight.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.BookingCommand;

namespace BookRight.Application.Commands.BookingCommands
{
    public class CompleteBookingCommandHandler : ICompleteBookingUseCase
    {
        private readonly IBookingRepository _bookingRepository;

        public CompleteBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task ExecuteAsync(CompleteBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found.");

            booking.Complete();

            await _bookingRepository.SaveChangesAsync();
        }
    }
}
