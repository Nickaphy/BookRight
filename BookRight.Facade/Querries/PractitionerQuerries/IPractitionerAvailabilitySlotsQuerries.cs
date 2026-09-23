using BookRight.Facade.Dtos.QuerryDto.PractitionerQuerry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Querries.PractitionerQuerries
{
    public interface IPractitionerAvailabilitySlotsQuerries
    {
        Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(
            Guid practitionerId,
            Guid clinicId,
            DateOnly week,
            int durationMinutes,
            CancellationToken cancellationToken = default);
    }
}
