using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.CommandDto.ClinicCommand
{
    public record UpdateClinicRequest(Guid Id,
                                       string Name,
                                       string Street,
                                       string City,
                                       string Zipcode,
                                       int AmountTreatmentRooms,
                                       CreateOpeningHourRequest[] OpeningHours  //Is Reused for creating opening hours
                                       );
    
}
