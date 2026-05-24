using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerQuerry
{
    public record PractitionerAvailableSlotDto(DateTime Start,
                                   DateTime End,
                                   bool IsAvailable);
}
