// Registers Infrastructure services in dependency injection
// Example: DbContext and repository implementations
// Called from UI startup later



// This class will register Infrastructure dependencies.
// It connects Application abstractions to Infrastructure implementations.
//
// Example later:
// IBookingRepository -> BookingRepository
// ICustomerRepository -> CustomerRepository
// BookRightDbContext -> SQL Server database
//
// This keeps Application independent from EF Core and database details.