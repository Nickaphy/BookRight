using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Campaign;
using BookRight.Facade.Dtos.CommandDto.CampaignCommand;

namespace BookRight.Application.UseCases.CampaignUseCases;

public class DeleteCampaignHandler : IDeleteCampaign
{
    private readonly ICampaignRepository _campaignRepository;

    public DeleteCampaignHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public async Task HandleAsync(DeleteCampaignCommand command, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);

        if (campaign == null)
            throw new UseCaseException("Campaign not found.");

        await _campaignRepository.DeleteAsync(campaign, cancellationToken);
    }
}
