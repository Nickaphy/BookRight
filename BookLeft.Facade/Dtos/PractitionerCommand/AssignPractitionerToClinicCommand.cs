using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerCommand
{
    public record AssignPractitionerToClinicCommand(Guid PractitionerId, 
                                                    Guid ClinicId, 
                                                    DateTime Date);
}
