namespace BookRight.Facade.Dtos.CommandDto.CampaignCommand;

public record UpdateCampaignCommand(
    Guid CampaignId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);
