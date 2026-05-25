using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.ClinicQuerry;

namespace BookRight.Facade.Querries.ClinicQuerries
{
    public interface IClinicQuerries
    {
        Task<ClinicDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<ClinicDto>> GetAllAsync();

        // Returns only the clinics where the given practitioner has
        // PractitionerClinicDay assignments — used by the ClinicSelector step.
        Task<IReadOnlyList<ClinicDto>> GetByPractitionerAsync(Guid practitionerId,
            CancellationToken cancellationToken = default);
    }
}
