using BookRight.Facade.Dtos.PractitionerQuerry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Querries.PractitionerQuerries
{
    public interface IPractitionerQuerries
    {
        Task<PractitionerDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PractitionerDto>> GetAllAsync();
        Task<IReadOnlyList<PractitionerDto>> GetByAuthorizationType(string authorizationType);
        Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(Guid practitionerId,
                                                                                              DateOnly week,
                                                                                              int durationMinutes,
                                                                                              CancellationToken cancellationToken = default);
    }
}
