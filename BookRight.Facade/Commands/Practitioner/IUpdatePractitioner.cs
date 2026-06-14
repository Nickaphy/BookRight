using BookRight.Facade.Dtos.CommandDto.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Practitioner
{
    public interface IUpdatePractitioner
    {
        Task HandleAsync(UpdatePractitionerCommand command, CancellationToken cancellationToken = default);
    }
}
