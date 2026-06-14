namespace BookRight.Facade.Dtos.QuerryDto.CampaignQuery;

public interface ICampaignQueries
{
    Task<CampaignDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CampaignDto>> GetAllAsync();
}
