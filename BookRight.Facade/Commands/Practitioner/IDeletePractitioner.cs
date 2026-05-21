using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Facade.Commands.Practitioner
{
    public interface IDeletePractitioner
    {
        Task HandleAsync(DeletePractitionerCommand command, CancellationToken cancellationToken = default);
    }
}
