using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CustomerDtos
{
    public record CustomerDto(
        Guid Id,
    string Name,
    string PhoneNumber,
    CustomerLoyaltyLevel LoyaltyLevel);

}
