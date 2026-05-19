using BookRight.Facade.Dtos.CampaignQuery;

namespace BookRight.Facade.Queries.CampaignQueries;

public interface ICampaignQueries
{
    Task<CampaignDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CampaignDto>> GetAllAsync();
}