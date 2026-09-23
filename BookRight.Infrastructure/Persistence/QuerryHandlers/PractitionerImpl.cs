using BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class PractitionerImpl : IPractitionerQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;
        private readonly IPractitionerAvailabilitySlotsQuerries _availabilitySlots;

        public PractitionerImpl(IDbContextFactory<AppDbContext> factory, IPractitionerAvailabilitySlotsQuerries availabilitySlots)
        {
            _factory = factory;
            _availabilitySlots = availabilitySlots;
        }

        public async Task<PractitionerDto?> GetByIdAsync(Guid id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Practitioners
                 .AsNoTracking()
                 .Where(p => p.Id == id)
                 .Select(p => new PractitionerDto(
                     p.Id,
                     p.Name,
                     p.Email,
                     p.PhoneNumber,
                     p.AuthorizationCode,
                     (PractitionerAuthorization)p.AuthorizationType))
                 .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<PractitionerDto>> GetAllAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Practitioners
                .AsNoTracking()
                .Select(p => new PractitionerDto(
                    p.Id,
                    p.Name,
                    p.Email,
                    p.PhoneNumber,
                    p.AuthorizationCode,
                    (PractitionerAuthorization)p.AuthorizationType))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PractitionerDto>> GetByAuthorizationType(string authorizationType)
        {
            using var context = _factory.CreateDbContext();
            var practitioners = await context.Practitioners
               .AsNoTracking()
               .Where(p => p.AuthorizationType.ToString() == authorizationType)
               .ToListAsync();

            return practitioners.Select(p => new PractitionerDto(
                p.Id,
                p.Name,
                p.Email,
                p.PhoneNumber,
                p.AuthorizationCode,
                (PractitionerAuthorization)p.AuthorizationType))
                .ToList();
        }

        
    }
}