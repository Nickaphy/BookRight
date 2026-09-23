using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry;
using BookRight.Facade.Querries.PractitionerQuerries;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.QuerryHandlers
{
    public sealed class PractitionerAvailabilitySlotsHandler : IPractitionerAvailabilitySlotsQuerries
    {
        private readonly IPractitionerClinicDayRepository _clinicDayRepository;
        private readonly IClinicRepository _clinicRepository;
        private readonly IBookingRepository _bookingRepository;

        public PractitionerAvailabilitySlotsHandler(
            IPractitionerClinicDayRepository clinicDayRepository,
            IClinicRepository clinicRepository,
            IBookingRepository bookingRepository)
        {
            _clinicDayRepository = clinicDayRepository;
            _clinicRepository = clinicRepository;
            _bookingRepository = bookingRepository;
        }
        public async Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(
        Guid practitionerId,
        Guid clinicId,
        DateOnly week,
        int durationMinutes,
        CancellationToken cancellationToken = default)
        {
            var weekStart = week.ToDateTime(TimeOnly.MinValue);
            var weekEnd = weekStart.AddDays(7);

            var clinicDays = await _clinicDayRepository.GetByPractitionerAndClinicInRangeAsync(
                practitionerId, clinicId, weekStart, weekEnd, cancellationToken);

            var clinic = await _clinicRepository.GetWithOpeningHoursAsync(clinicId, cancellationToken);
            if (clinic is null)
                return [];

            var bookings = await _bookingRepository.GetForPractitionerAndClinicInRangeAsync(
                practitionerId, clinicId, weekStart, weekEnd, cancellationToken);

            var bookingList = bookings.ToList();
            var slots = new List<PractitionerAvailableSlotDto>();

            foreach (var clinicDay in clinicDays)
            {
                var openingHour = clinic.OpeningHours.FirstOrDefault(oh => oh.WeekDay == clinicDay.Date.DayOfWeek);
                if (openingHour is null) continue;

                slots.AddRange(GenerateSlotsForDay(clinicDay, openingHour, bookingList, durationMinutes));
            }

            return slots.OrderBy(s => s.Start).ToList();
        }
        

        private static List<PractitionerAvailableSlotDto> GenerateSlotsForDay(
            PractitionerClinicDay clinicDay,
            ClinicOpeningHour openingHour,
            IReadOnlyList<Booking> bookings,
            int durationMinutes)
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
                    slotStart = slotStart.AddMinutes(15);
                    continue;
                }

                var teamBooking = FindTeamBooking(bookings, slotStart);
                var isBooked = IsSlotBooked(bookings, slotStart, slotEnd);

                slots.Add(new PractitionerAvailableSlotDto(slotStart, slotEnd, !isBooked, teamBooking != null, teamBooking?.Id));
                slotStart = slotStart.AddMinutes(15);
            }

            return slots;
        }

        private static bool IsSlotBooked(IReadOnlyList<Booking> bookings, DateTime slotStart, DateTime slotEnd)
        {
            return bookings.Any(b =>
                b.TimeRange.Start < slotEnd &&
                b.TimeRange.End > slotStart);
        }
        private static Booking? FindTeamBooking(IReadOnlyList<Booking> bookings, DateTime slotStart)
        {
            return bookings.FirstOrDefault(b =>
                b.TimeRange.Start <= slotStart &&
                b.TimeRange.End > slotStart &&
                b.IsTeam);
        }

    }
}

