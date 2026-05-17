using BookRight.Domain.Exceptions;

namespace BookRight.Domain.Entities.Clinics
{
    public class ClinicOpeningHour
    {
        public DayOfWeek WeekDay { get; private set; }
        public TimeOnly OpeningTime { get; private set; }
        public TimeOnly ClosingTime { get; private set; }
        public ClinicOpeningHour(DayOfWeek weekDay, TimeOnly openingTime, TimeOnly closingTime)
        {
            WeekDay = weekDay;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            ValidateOpeningHours();
        }

        public void ValidateOpeningHours()
        {
            if (OpeningTime >= ClosingTime)
            {
                throw new DomainException("Opening time must be before closing time.");
            }
        }
        public void UpdateOpeningHours(TimeOnly openingTime, TimeOnly closingTime)
        {
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            ValidateOpeningHours();
        }
    }
}