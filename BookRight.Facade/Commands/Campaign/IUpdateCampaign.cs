using BookRight.Facade.Dtos.CampaignCommand;

namespace BookRight.Facade.Commands.Campaign;

public interface IUpdateCampaign
{
    Task HandleAsync(UpdateCampaignCommand command, CancellationToken cancellationToken = default);
}
