using BookRight.Facade.Dtos.QuerryDto.ClinicQuerry;
using BookRight.Facade.Querries.ClinicQuerries;
using BookRight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class ClinicImpl : IClinicQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public ClinicImpl(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<ClinicDto?> GetByIdAsync(Guid id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Clinics
                .AsNoTracking()
                .Include(c => c.OpeningHours)
                .Where(c => c.Id == id)
                .Select(c => new ClinicDto(
                    c.Id,
                    c.Name,
                    c.Street,
                    c.City,
                    c.Zipcode,
                    c.AmountTreatmentRooms,
                    c.OpeningHours.Select(oh => new OpeningHourDto(oh.WeekDay, oh.OpeningTime, oh.ClosingTime)).ToArray()))
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<ClinicDto>> GetAllAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Clinics
                .AsNoTracking()
                .Include(c => c.OpeningHours)
                .Select(c => new ClinicDto(
                    c.Id,
                    c.Name,
                    c.Street,
                    c.City,
                    c.Zipcode,
                    c.AmountTreatmentRooms,
                    c.OpeningHours.Select(oh => new OpeningHourDto(oh.WeekDay, oh.OpeningTime, oh.ClosingTime)).ToArray()))
                .ToListAsync();
        }

        // Returns only the clinics where the practitioner has scheduled days,
        // so the ClinicSelector only shows clinics actually relevant to them.
        public async Task<IReadOnlyList<ClinicDto>> GetByPractitionerAsync(
            Guid practitionerId,
            CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();

            var clinicIds = await context.PractitionerClinicDays
                .AsNoTracking()
                .Where(pc => pc.PractitionerId == practitionerId)
                .Select(pc => pc.ClinicId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return await context.Clinics
                .AsNoTracking()
                .Include(c => c.OpeningHours)
                .Where(c => clinicIds.Contains(c.Id))
                .OrderBy(c => c.Name)
                .Select(c => new ClinicDto(
                    c.Id,
                    c.Name,
                    c.Street,
                    c.City,
                    c.Zipcode,
                    c.AmountTreatmentRooms,
                    c.OpeningHours.Select(oh => new OpeningHourDto(oh.WeekDay, oh.OpeningTime, oh.ClosingTime)).ToArray()))
                .ToListAsync(cancellationToken);
        }
    }
}
