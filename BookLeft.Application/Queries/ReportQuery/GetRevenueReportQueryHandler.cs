// Query handler for revenue reporting
// Later uses LINQ GroupBy, Sum, Count and projections


/*

context.Bookings
  .Where(...)
  .ToList()


GroupBy
Select
Count
Sum
Average
*/



// QUERY HANDLER
// Responsible for reading data only

// Flow:
// 1. Fetch data from repository
// 2. Use LINQ to group and aggregate data
// 3. Map result to DTO
// 4. Return result

// Important:
// Uses IQueryable for database execution
// Avoids loading unnecessary data

// IMPORTANT:
// Keep query as IQueryable as long as possible
// Avoid calling ToList() too early
//
// Correct flow:
// Database handles filtering, grouping and aggregation
//
// Incorrect:
// Loading all data into memory before processing