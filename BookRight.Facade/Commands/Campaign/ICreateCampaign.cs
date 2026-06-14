using BookRight.Facade.Dtos.CommandDto.CampaignCommand;

namespace BookRight.Facade.Commands.Campaign;

public interface ICreateCampaign
{
    Task HandleAsync(CreateCampaignCommand command, CancellationToken cancellationToken = default);
}
