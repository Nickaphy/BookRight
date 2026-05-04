// Aggregate Root
// Responsible for creating and managing bookings

// Rules:
// - Cannot overlap bookings for the same practitioner
// - Must respect clinic capacity
// - Must calculate final price
// - Must use best discount



// Booking is the main aggregate root
// Responsible for managing appointment creation and validation

// Will later contain:
// - TimeRange
// - Customer reference
// - Practitioner reference
// - Clinic reference
// - BookingLines
// - Price calculation
// - Status