// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain
using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
        
    }
    public async Task UpdateAsync(Practitioner practitioner,
                     CancellationToken cancellationToken = default)
    {
        _context.Practitioners.Update(practitioner);  //hvad er dette!!!!

    }

    public async Task DeleteAsync(Practitioner practitioner,
                      CancellationToken cancellationToken = default)
    {
        
        _context.Practitioners.Remove(practitioner);
       
    }
    public async Task SaveAsync(Practitioner practitioner, CancellationToken cancellationToken)
    {
            await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<int> CountPractitionersByClinicAndDateAsync(Guid clinicId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _context.Practitioners
            .Where(p => p.ClinicDays.Any(cd => cd.ClinicId == clinicId && cd.Date == date.Date))
            .CountAsync(cancellationToken);
    }
}
