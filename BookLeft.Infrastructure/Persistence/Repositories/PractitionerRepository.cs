// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain
using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Infrastructure.Persistence;
using System.Numerics;
using System.Threading;

namespace BookRight.Infrastructure.Persistece.Repository;
public class PractitionerRepository : IPractitionerRepository
{
    private readonly AppDbContext _context;

    public PractitionerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Practitioner?> GetByIdAsync(Guid id,
                     CancellationToken cancellationToken = default)
    {
        return await _context.Practitioners.FindAsync(id, cancellationToken);
    }
    
    public async Task AddAsync(Practitioner practitioner,
                     CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(practitioner, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Practitioner practitioner,
                     CancellationToken cancellationToken = default)
    {
        _context.Set<Practitioner>().Update(practitioner);  //hvad er dette!!!!
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Practitioner practitioner,
                      CancellationToken cancellationToken = default)
    {
        _context.Practitioners.Remove(practitioner);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
