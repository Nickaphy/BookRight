using BookRight.Facade.Dtos.CommandDto.ClinicCommand;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BookRight.Facade.Commands.Clinic
{
    public interface ICreateClinic
    {
        Task HandleAsync(CreateClinicRequest request, CancellationToken cancellationToken = default);
    }
}
