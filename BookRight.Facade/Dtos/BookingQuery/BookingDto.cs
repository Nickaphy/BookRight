namespace BookRight.Facade.Dtos.BookingQuery;

public record BookingDto(
    Guid Id,
    string CustomerName,
    string TreatmentName,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    Guid ClinicId,
    string ClinicName,
    string PractitionerName,
    decimal FinalPrice,
    string DiscountType);