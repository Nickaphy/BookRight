namespace BookRight.Facade.Dtos.Reports;

public record ClinicReportDto(
    string ClinicName,
    int TotalBookings,
    int CompletedBookings,
    int CancelledBookings,
    int NoShowBookings,
    int PendingBookings,
    decimal TotalRevenue,
    decimal AverageBookingValue,
    string MostPopularTreatment,
    int UniqueCustomers);