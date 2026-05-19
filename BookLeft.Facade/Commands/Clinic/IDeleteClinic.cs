using BookRight.Facade.Dtos.ClinicCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Clinic
{
    public interface IDeleteClinic
    {
        Task HandleAsync(DeleteClinicRequest request, CancellationToken cancellationToken = default);
    }
}
