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