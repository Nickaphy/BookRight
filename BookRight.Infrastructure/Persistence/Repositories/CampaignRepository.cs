// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Campaigns;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly AppDbContext _context;
    public CampaignRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Campaign?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Campaigns.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        _context.Campaigns.Add(campaign);
    }


    public async Task UpdateAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        _context.Campaigns.Update(campaign);

    }

    public async Task DeleteAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        _context.Campaigns.Remove(campaign);

    }

    public async Task SaveAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Campaigns
        .AnyAsync(c => c.Name == name, cancellationToken);
    }
}