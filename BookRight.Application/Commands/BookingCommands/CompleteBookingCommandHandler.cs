using BookRight.Application.Repositories;
using BookRight.Domain.Common;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.BookingCommand;

namespace BookRight.Application.Commands.BookingCommands
{
    public class CompleteBookingCommandHandler : ICompleteBookingUseCase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public CompleteBookingCommandHandler(
            IBookingRepository bookingRepository,
            IDomainEventDispatcher domainEventDispatcher)
        {
            _bookingRepository = bookingRepository;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task ExecuteAsync(CompleteBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found.");

            booking.Complete();

            await _bookingRepository.SaveChangesAsync();

            foreach (var domainEvent in booking.DomainEvents)
                await _domainEventDispatcher.Dispatch(domainEvent);

            booking.ClearDomainEvents();
        }
    }
}
