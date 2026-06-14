using BookRight.Facade.Dtos.QuerryDto.BookingPricePreviewDto;

namespace BookRight.Facade.Querries.BookingQuerries;

public interface IBookingPricingFacade
{

    // Calculates the expected price for a booking before it is saved.
    // Runs the full discount strategy pipeline so the UI can show the
    // customer an accurate price preview on the confirmation screen.

    Task<BookingPricePreviewDto> GetPricePreviewAsync(
        Guid customerId,
        Guid treatmentTypeId,
        DateTime startTime,
        CancellationToken cancellationToken = default);
}