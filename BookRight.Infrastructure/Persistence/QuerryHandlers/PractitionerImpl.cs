using BookRight.Facade.Dtos.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class PractitionerImpl : IPractitionerQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public PractitionerImpl(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
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
        public async Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(Guid practitionerId,
                                                                                              DateOnly week,
                                                                                              int durationMinutes,
                                                                                              CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();

            var weekStart = week.ToDateTime(TimeOnly.MinValue);
            var weekEnd = weekStart.AddDays(7);

            var clinicDays = await context.PractitionerClinicDays
                .AsNoTracking()
                .Where(pc => pc.PractitionerId == practitionerId
                          && pc.Date >= weekStart
                          && pc.Date < weekEnd)
                .ToListAsync(cancellationToken);

            // Hent klinikkerne med åbningstider
            var clinicIds = clinicDays.Select(pc => pc.ClinicId).Distinct();
            var clinics = await context.Clinics
                .AsNoTracking()
                .Include(c => c.OpeningHours)
                .Where(c => clinicIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            // Hent eksisterende bookings for practitioner i ugen
            var bookings = await context.Bookings
                .AsNoTracking()
                .Where(b => b.PractitionerId == practitionerId
                         && b.TimeRange.Start >= weekStart
                         && b.TimeRange.Start < weekEnd)
                .ToListAsync(cancellationToken);


            var slots = new List<PractitionerAvailableSlotDto>();

            foreach (var clinicDay in clinicDays)
            {
                var clinic = clinics.FirstOrDefault(c => c.Id == clinicDay.ClinicId);
                if (clinic is null) continue;

                var dayOfWeek = clinicDay.Date.DayOfWeek;
                var openingHour = clinic.OpeningHours.FirstOrDefault(oh => oh.WeekDay == dayOfWeek);
                if (openingHour is null) continue;

                var slotStart = clinicDay.Date.Date + openingHour.OpeningTime.ToTimeSpan();
                var closingTime = clinicDay.Date.Date + openingHour.ClosingTime.ToTimeSpan();

                while (slotStart.AddMinutes(durationMinutes) <= closingTime)
                {
                    var slotEnd = slotStart.AddMinutes(durationMinutes);

                    var isBooked = bookings.Any(b =>
                        b.TimeRange.Start < slotEnd &&
                        b.TimeRange.End > slotStart);

                    slots.Add(new PractitionerAvailableSlotDto(slotStart, slotEnd, !isBooked));

                    slotStart = slotStart.AddMinutes(durationMinutes);
                }
            }

            return slots.OrderBy(s => s.Start).ToList();


        }
    }
}
