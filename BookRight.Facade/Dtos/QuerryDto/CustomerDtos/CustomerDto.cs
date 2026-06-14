using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.QuerryDto.CustomerDtos
{
    public record CustomerDto(
        Guid Id,
        string Name,
        string PhoneNumber,
        CustomerLoyaltyLevel LoyaltyLevel,
        DateTime DateOfBirth);
}