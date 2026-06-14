using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry
{
    public record PractitionerDto(Guid id,
                                  string name,
                                  string email,
                                  string phoneNumber,
                                  string authorizationCode,
                                  PractitionerAuthorization Authorization)
    {

    }
}
