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
    }
}
