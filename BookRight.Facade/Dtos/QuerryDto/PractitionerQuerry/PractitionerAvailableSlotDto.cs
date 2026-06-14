using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry
{
    public record PractitionerAvailableSlotDto(DateTime Start,
                                   DateTime End,
                                   bool IsAvailable,
                                   bool IsTeam);
}
