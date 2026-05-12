using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.PractitionerQuerry;

namespace BookRight.Facade.Querries.PractitionerQuerries
{
    public interface IPractitionerQuerries
    {
        Task<PractitionerDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PractitionerDto>> GetAllAsync();
    }
}
