using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.PractitionerCommand
{
    public record CreatePractitionerCommand(string Name,
                                            string Email,
                                            string PhoneNumber,
                                            string AuthorizationCode,
                                            string AuthorizationType);
}
