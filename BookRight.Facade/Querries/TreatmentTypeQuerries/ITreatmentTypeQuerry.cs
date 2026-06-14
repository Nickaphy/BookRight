using BookRight.Facade.Dtos.QuerryDto.TreatmentTypeDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Querries.TreatmentTypeQuerries
{
    public interface ITreatmentTypeQuerry
    {
        Task<IReadOnlyList<TreatmentTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
