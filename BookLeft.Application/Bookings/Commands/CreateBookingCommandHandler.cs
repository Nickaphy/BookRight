// Command handler
// Executes the use case
// Coordinates repositories, domain rules and services
// Does not contain UI code



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


/*
CreateBookingCommandHandler
→ bruger Domain regler
→ kalder repository
→ SaveChanges()
*/



// COMMAND HANDLER
// Responsible for changing system state

// Flow:
// 1. Validate input
// 2. Check domain rules (overlap, capacity, practitioner availability)
// 3. Calculate price (using discount strategies)
// 4. Create Booking entity
// 5. Save using repository