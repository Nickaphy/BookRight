using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerCommand
{
    public record UpdatePractitionerCommand(Guid PractitionerId,
                                            string Name,
                                            string PhoneNumber,
                                            string Email,
                                            string AuthorizationCode,
                                            string AuthorizationType)
    {

    }
}
