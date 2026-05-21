using BookRight.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Domain.Common;

namespace BookRight.Domain.ValueObjects
{
    public class ClinicOpeningHour : ValueObject
    {
        public DayOfWeek WeekDay { get; init; }
        public TimeOnly OpeningTime { get; init; }
        public TimeOnly ClosingTime { get; init; }
        public ClinicOpeningHour(DayOfWeek weekDay, TimeOnly openingTime, TimeOnly closingTime)
        {
            WeekDay = weekDay;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            ValidateOpeningHours();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return WeekDay;
            yield return OpeningTime;
            yield return ClosingTime;
        }
        public void ValidateOpeningHours()
        {
            if (OpeningTime >= ClosingTime)
            {
                throw new DomainException("Opening time must be before closing time.");
            }
        }

    }
}
