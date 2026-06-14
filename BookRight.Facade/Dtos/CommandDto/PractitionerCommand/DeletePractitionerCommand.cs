using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.PractitionerCommand
{
    public record DeletePractitionerCommand(Guid PractitionerId);
}
