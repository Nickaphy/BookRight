using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Infrastructure.Persistence.QuerryHandlers
{
    public class PractitionerAvailabilitySlots : IPractitionerAvailabilitySlotsQuerries
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public PractitionerAvailabilitySlots(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }


        private static bool IsSlotBooked(IReadOnlyList<Booking> bookings, DateTime slotStart, DateTime slotEnd)
        {
            return bookings.Any(b =>
                b.TimeRange.Start < slotEnd &&
                b.TimeRange.End > slotStart);
        }

        private static bool IsTeamSlot(IReadOnlyList<Booking> bookings, DateTime slotStart)
        {
            return bookings.Any(b =>
                b.TimeRange.Start <= slotStart &&
                b.TimeRange.End > slotStart &&
                b.IsTeam);
        }

        private async Task<List<PractitionerClinicDay>> FetchClinicDaysAsync(Guid practitionerId, Guid clinicId,
        DateTime weekStart, DateTime weekEnd, CancellationToken cancellationToken)
        {
            using var context = _factory.CreateDbContext();
            return await context.PractitionerClinicDays
                .AsNoTracking()
                .Where(pc => pc.PractitionerId == practitionerId
                          && pc.ClinicId == clinicId
                          && pc.Date >= weekStart
                          && pc.Date < weekEnd)
                .ToListAsync(cancellationToken);
        }


        private async Task<Clinic?> FetchClinicWithOpeningHoursAsync(Guid clinicId, CancellationToken cancellationToken)
        {
            using var context = _factory.CreateDbContext();
            return await context.Clinics
                .AsNoTracking()
                .Include(c => c.OpeningHours)
                .FirstOrDefaultAsync(c => c.Id == clinicId, cancellationToken);
        }


        private async Task<List<Booking>> FetchBookingsAsync(Guid practitionerId, Guid clinicId,
        DateTime weekStart, DateTime weekEnd, CancellationToken cancellationToken)
        {
            using var context = _factory.CreateDbContext();
            return await context.Bookings
                .AsNoTracking()
                .Where(b => b.PractitionerId == practitionerId
                         && b.ClinicId == clinicId
                         && b.TimeRange.Start >= weekStart
                         && b.TimeRange.Start < weekEnd
                         && (b.Status == Domain.Enums.BookingStatus.Created ||
                             b.Status == Domain.Enums.BookingStatus.Completed))
                .ToListAsync(cancellationToken);
        }


        public async Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(
        Guid practitionerId, Guid clinicId, DateOnly week, int durationMinutes,
        CancellationToken cancellationToken = default)
        {
            using var context = _factory.CreateDbContext();

            var weekStart = week.ToDateTime(TimeOnly.MinValue);
            var weekEnd = weekStart.AddDays(7);

            var clinicDays = await FetchClinicDaysAsync(practitionerId, clinicId, weekStart, weekEnd, cancellationToken);
            var clinic = await FetchClinicWithOpeningHoursAsync(clinicId, cancellationToken);

            if (clinic == null)
                return [];

            var bookings = await FetchBookingsAsync(practitionerId, clinicId, weekStart, weekEnd, cancellationToken);

            var slots = new List<PractitionerAvailableSlotDto>();

            foreach (var clinicDay in clinicDays)
            {
                var openingHour = clinic.OpeningHours.FirstOrDefault(oh => oh.WeekDay == clinicDay.Date.DayOfWeek);
                if (openingHour is null) continue;

                slots.AddRange(GenerateSlotsForDay(clinicDay, openingHour, bookings, durationMinutes));
            }

            return slots.OrderBy(s => s.Start).ToList();
        }

        private List<PractitionerAvailableSlotDto> GenerateSlotsForDay(PractitionerClinicDay clinicDay,
        ClinicOpeningHour openingHour, IReadOnlyList<Booking> bookings, int durationMinutes)
        {
            var slots = new List<PractitionerAvailableSlotDto>();

            var slotStart = clinicDay.Date.Date + openingHour.OpeningTime.ToTimeSpan();
            var closingTime = clinicDay.Date.Date + openingHour.ClosingTime.ToTimeSpan();
            var now = DateTime.Now;

            while (slotStart.AddMinutes(durationMinutes) <= closingTime)
            {
                var slotEnd = slotStart.AddMinutes(durationMinutes);

                if (slotStart < now)
                {
                    slotStart = slotStart.AddMinutes(durationMinutes);
                    continue;
                }

                var isBooked = IsSlotBooked(bookings, slotStart, slotEnd);
                var isTeam = IsTeamSlot(bookings, slotStart);

                slots.Add(new PractitionerAvailableSlotDto(slotStart, slotEnd, !isBooked, isTeam));
                slotStart = slotStart.AddMinutes(15);
            }

            return slots;
        }

       
    }
}
