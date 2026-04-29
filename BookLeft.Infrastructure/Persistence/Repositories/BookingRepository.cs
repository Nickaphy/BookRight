// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain



// Repository implementation using EF Core
//
// Responsibilities:
// - Query data using LINQ
// - Save data using DbContext
//
// Important:
// - Uses IQueryable for efficient queries
// - Avoids loading unnecessary data
// - Calls SaveChangesAsync for persistence
//
// This is where EF Core meets Application layer