namespace BookRight.Facade.Dtos.QuerryDto.BookingPricePreviewDto;

public record BookingPricePreviewDto(
    decimal BasePrice,
    decimal DiscountAmount,
    decimal FinalPrice,
    string DiscountLabel   // e.g. "Bronze loyalitetsrabat (5%)" or "Ingen rabat"
);