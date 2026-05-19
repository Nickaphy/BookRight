using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CustomerCommands
{
    public record UpdateCustomerRequest(
        Guid CustomerId,
        string Name,
        string PhoneNumber,
        string Email,
        int LoyaltyLevel,
        DateTime DateOfBirth,
        string Note,
        string Street,
        string City,
        string Zipcode
    );
    
}
