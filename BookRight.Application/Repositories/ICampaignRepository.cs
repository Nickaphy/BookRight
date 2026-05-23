using BookRight.Domain.Entities.Campaigns;

namespace BookRight.Application.Repositories;

public interface ICampaignRepository
{
    //CancellationToken acts like an alarm to stop code from running if no one is listening preventing
    //leaving it in a broken state.
    Task<Campaign?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Campaign campaign, CancellationToken cancellationToken = default);
    Task UpdateAsync(Campaign campaign, CancellationToken cancellationToken = default);
    Task DeleteAsync(Campaign campaign, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
}
