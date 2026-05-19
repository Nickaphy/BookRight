using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.ClinicQuerry
{
    public record OpeningHourDto(DayOfWeek Weekday, TimeOnly OpeningTime, TimeOnly ClosingTime);
    
}
