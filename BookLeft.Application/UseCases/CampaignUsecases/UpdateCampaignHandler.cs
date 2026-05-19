using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Campaign;
using BookRight.Facade.Dtos.CampaignCommand;

namespace BookRight.Application.UseCases.CampaignUseCases;

public class UpdateCampaignHandler : IUpdateCampaign
{
    private readonly ICampaignRepository _campaignRepository;

    public UpdateCampaignHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public async Task HandleAsync(UpdateCampaignCommand command, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);

        if (campaign == null)
            throw new UseCaseException("Campaign not found.");

        campaign.Update(command.Name, command.StartDate, command.EndDate, command.DiscountPercent);

        await _campaignRepository.UpdateAsync(campaign, cancellationToken);
    }
}