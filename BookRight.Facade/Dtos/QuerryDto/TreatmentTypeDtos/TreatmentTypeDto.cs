using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Dtos.QuerryDto.TreatmentTypeDtos
{
    public record TreatmentTypeDto(
    Guid Id,
    string Name,
    int DurationMinutes,
    decimal BasePrice,
    string AuthorizationType,
    int MaxParticipants
    );

}

