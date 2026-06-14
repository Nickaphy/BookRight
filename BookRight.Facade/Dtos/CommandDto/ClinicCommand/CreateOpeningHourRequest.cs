using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.ClinicCommand
{
    public record CreateOpeningHourRequest(DayOfWeek WeekDay,
                                           TimeOnly OpeningTime,
                                           TimeOnly ClosingTime);
}
