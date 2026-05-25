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

        // ClinicId filters slots to only that clinic's opening hours.
        Task<IReadOnlyList<PractitionerAvailableSlotDto>> GetAvailableSlotsAsync(
            Guid practitionerId,
            Guid clinicId,
            DateOnly week,
            int durationMinutes,
            CancellationToken cancellationToken = default);
    }
}
