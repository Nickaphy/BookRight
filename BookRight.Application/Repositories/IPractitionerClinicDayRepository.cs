using BookRight.Domain.Entities.Practitioners;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Repositories
{
    public interface IPractitionerClinicDayRepository
    {
        Task<int> CountByClinicAndDateAsync(Guid clinicId, DateTime date, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PractitionerClinicDay>> GetByPractitionerAndClinicInRangeAsync(
            Guid practitionerId,
            Guid clinicId,
            DateTime rangeStart,
            DateTime rangeEnd,
            CancellationToken cancellationToken = default);
    }
}
