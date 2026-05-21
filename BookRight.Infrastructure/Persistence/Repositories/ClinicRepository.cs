// Repository implementation
// Implements Application repository interfaces
// Uses EF Core DbContext internally
// Keeps database access separated from Application and Domain
using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Clinics;

namespace BookRight.Infrastructure.Persistence.Repositories;

public class ClinicRepository : IClinicRepository
{
    private readonly AppDbContext _context;
    public ClinicRepository(AppDbContext context)
    {
        _context = context;
    }
    // Implement repository methods (e.g., Add, GetById, GetAll, Update, Delete)
    // Use _context to interact with the database

    public async Task AddAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        _context.Clinics.Add(clinic);
        
    }

    public async Task<Clinic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clinics.FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        _context.Clinics.Update(clinic);
        
    }

    public async Task DeleteAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        _context.Clinics.Remove(clinic);
            
    }

    public async Task SaveAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

}
