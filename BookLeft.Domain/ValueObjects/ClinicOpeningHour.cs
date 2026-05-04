namespace BookRight.Domain.Entities.Clinics
{
    public class ClinicOpeningHour
    {
        public DayOfWeek WeekDay { get; private set; }
        public DateTime OpeningTime { get; private set; }
        public DateTime ClosingTime { get; private set; }
    }
}