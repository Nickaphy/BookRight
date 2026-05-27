using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Repositories
{
    public interface IPractitionerClinicDayRepository
    {
        Task<int> CountByClinicAndDateAsync(Guid clinicId, DateTime date, CancellationToken cancellationToken = default);
    }
}
