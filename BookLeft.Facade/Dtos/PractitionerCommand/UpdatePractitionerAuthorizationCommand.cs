using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookRight.Facade.Dtos.PractitionerCommand
{
    public record UpdatePractitionerAuthorizationCommand(Guid PractitionerId, 
                                                         string AuthorizationCode, 
                                                         string AuthorizationType);
}
