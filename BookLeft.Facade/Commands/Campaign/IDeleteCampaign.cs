using BookRight.Facade.Dtos.CampaignCommand;

namespace BookRight.Facade.Commands.Campaign;

public interface IDeleteCampaign
{
    Task HandleAsync(DeleteCampaignCommand command, CancellationToken cancellationToken = default);
}