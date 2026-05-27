using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.BookingCommand;

namespace BookRight.Application.Commands.BookingCommands;

public class CancelBookingCommandHandler : ICancelBookingFacade
{
    private readonly IBookingRepository _bookingRepo;

    public CancelBookingCommandHandler(IBookingRepository bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    public async Task ExecuteAsync(CancelBookingRequest request)
    {
        var booking = await _bookingRepo.GetByIdAsync(request.BookingId)
            ?? throw new UseCaseException($"Booking with ID {request.BookingId} not found.");

        booking.Cancel(); // Kalder cancel metoden paa booking entiteten for at andre status til annulleret

        await _bookingRepo.UpdateAsync(booking); // Opdaterer bookingen i databasen
        await _bookingRepo.SaveChangesAsync(); // Gemmer �ndringerne i databasen
    }
}
