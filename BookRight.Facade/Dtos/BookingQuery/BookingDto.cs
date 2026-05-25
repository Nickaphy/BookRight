namespace BookRight.Facade.Dtos.BookingQuery;

public record BookingDto(
    Guid Id,
    string CustomerName,
    string TreatmentName,
    DateTime StartTime,
    DateTime EndTime,
    string Status);
