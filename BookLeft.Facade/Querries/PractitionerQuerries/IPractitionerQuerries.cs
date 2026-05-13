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
    }
}
