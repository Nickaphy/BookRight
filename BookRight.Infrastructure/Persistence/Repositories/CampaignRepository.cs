// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain

using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Clinics;

namespace BookRight.Infrastructure.Persistence.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly AppDbContext _context;
    public CampaignRepository(AppDbContext context)
    {
        _context = context;
    }
}