using BookRight.Facade.Dtos.CommandDto.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Practitioner
{
    public interface IAssignPractitionerToClinic
    {
        Task HandleAsync(AssignPractitionerToClinicCommand command, CancellationToken cancellationToken = default);
    }
}
