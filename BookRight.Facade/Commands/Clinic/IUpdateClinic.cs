using BookRight.Facade.Dtos.CommandDto.ClinicCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Clinic
{
    public interface IUpdateClinic
    {
        Task HandleAsync(UpdateClinicRequest request, CancellationToken cancellationToken = default);
    }
}
