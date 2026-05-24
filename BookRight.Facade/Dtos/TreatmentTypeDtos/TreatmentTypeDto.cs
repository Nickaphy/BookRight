using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.TreatmentTypeDtos
{
    public record TreatmentTypeDto(
    Guid Id,
    string Name,
    int DurationMinutes,
    decimal BasePrice,
    string AuthorizationType
    );

}

