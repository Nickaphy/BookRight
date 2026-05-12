using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerCommand
{
    public record UpdatePractitionerPhoneNumberCommand(Guid PractitionerId, 
                                                       string PhoneNumber);
}
