namespace BookRight.Facade.Dtos.CampaignCommand;

public record UpdateCampaignCommand(
    Guid CampaignId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);