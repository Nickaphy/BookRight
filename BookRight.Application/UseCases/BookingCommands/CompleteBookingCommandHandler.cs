using BookRight.Application.Repositories;
using BookRight.Domain.Common;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;

namespace BookRight.Application.UseCases.BookingCommands
{
    public class CompleteBookingCommandHandler : ICompleteBookingUseCase
    {
        private readonly IBookingRepository _bookingRepository;
        

        public CompleteBookingCommandHandler(
            IBookingRepository bookingRepository)
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
