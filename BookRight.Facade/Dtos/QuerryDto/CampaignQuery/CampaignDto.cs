namespace BookRight.Facade.Dtos.QuerryDto.CampaignQuery;

public record CampaignDto(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);
