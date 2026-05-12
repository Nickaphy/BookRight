// Facade interface
// This is the contract used by the Blazor UI
// UI should depend on this interface instead of Application handlers directly
// Keeps UI simple and separated from business logic



/*
CreateBooking.razor
↓
IBookingFacade
↓
CreateBookingCommand
↓
CreateBookingCommandHandler
↓
Booking domain model
↓
IBookingRepository
↓
BookingRepository
↓
BookRightDbContext
↓
SQL Server
*/

// Erik´s work.

using BookRight.Facade.Dtos;

namespace BookRight.Facade.Interfaces;

public interface IBookingFacade
{
    Task<Guid> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default);
}

// UI calls IBookingFacade -> Facade tranlates request into a command -> Application executes the Use Case.
