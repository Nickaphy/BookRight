namespace BookRight.Facade.Dtos.CommandDto.CampaignCommand;

public record CreateCampaignCommand(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int DiscountPercent);
