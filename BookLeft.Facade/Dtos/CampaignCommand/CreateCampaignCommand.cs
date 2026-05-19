namespace BookRight.Facade.Dtos.CampaignCommand;

public record CreateCampaignCommand(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);