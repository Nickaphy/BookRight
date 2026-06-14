using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.PractitionerCommand
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
