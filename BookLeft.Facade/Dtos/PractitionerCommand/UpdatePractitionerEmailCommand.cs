using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerCommand
{
    public record UpdatePractitionerEmailCommand(Guid PractitionerId, 
                                                 string Email);
}
