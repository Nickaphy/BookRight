namespace BookRight.Facade.Dtos.CampaignQuery;

public record CampaignDto(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);
