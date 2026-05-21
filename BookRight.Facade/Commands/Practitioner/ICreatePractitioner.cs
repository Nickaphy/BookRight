using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Practitioner
{
    public interface ICreatePractitioner
    {
        Task HandleAsync(CreatePractitionerCommand command, CancellationToken cancellationToken = default);
    }
}
