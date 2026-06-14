using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Campaigns;
using BookRight.Facade.Commands.Campaign;
using BookRight.Facade.Dtos.CommandDto.CampaignCommand;

namespace BookRight.Application.UseCases.CampaignUseCases;

public class CreateCampaignHandler : ICreateCampaign
{
    //Dependency inversion never creates itself we depend on the abstraction.
    private readonly ICampaignRepository _campaignRepository;

    //Constructor Injection
    public CreateCampaignHandler(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    //The method responsible for recieving the DTO from the facade with the information needed to create a campaign.
    public async Task HandleAsync(CreateCampaignCommand command, CancellationToken cancellationToken = default)
    {
        var exists = await _campaignRepository.ExistsAsync(command.Name, cancellationToken);


        if (exists)
            throw new InvalidOperationException($"That campaign = {command.Name} already exists");
        //Creating the campaign1
        var campaign = new Campaign(
            command.Name,
            command.StartDate,
            command.EndDate,
            command.DiscountPercent
        );

        //Hands the fresh campaign the repository to save it.
        await _campaignRepository.AddAsync(campaign, cancellationToken);
    }
}
