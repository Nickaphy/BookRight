// Erik's work.

using BookRight.Application.Commands.BookingCommands;
using BookRight.Facade.Dtos;
using BookRight.Facade.Interfaces;

namespace BookRight.Facade.Services;

public sealed class BookingFacade : IBookingFacade
{
    private readonly CreateBookingCommandHandler _createBookingCommandHandler;

    public BookingFacade(CreateBookingCommandHandler createBookingCommandHandler)
    {
        _createBookingCommandHandler = createBookingCommandHandler;
    }

    public async Task<Guid> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateBookingCommand(
            request.CustomerId,
            request.PractitionerId,
            request.ClinicId,
            request.TreatmentTypeId,
            request.StartTime);

        return await _createBookingCommandHandler.HandleAsync(command, cancellationToken);
    }
}