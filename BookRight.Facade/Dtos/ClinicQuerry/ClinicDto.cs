using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.ClinicQuerry
{
    public record ClinicDto(Guid Id,
                         string Name,
                         string Street,
                         string City,
                         string Zipcode,
                         int AmountTreatmentRooms,
                         OpeningHourDto[] OpeningHours
                         );
    
}
