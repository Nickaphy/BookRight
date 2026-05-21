using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases
{
    public class DeletePractitionerHandler : IDeletePractitioner
    {

        private readonly IPractitionerRepository _practitionerRepository;


        public DeletePractitionerHandler(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public async Task HandleAsync(DeletePractitionerCommand command, CancellationToken cancellationToken = default)
        {
            var practitioner = await _practitionerRepository.GetByIdAsync(command.PractitionerId, cancellationToken);

            if (practitioner == null)
            {
                throw new UseCaseException("Practitioner not found.");
            }

            await _practitionerRepository.DeleteAsync(practitioner, cancellationToken);
            await _practitionerRepository.SaveAsync(practitioner, cancellationToken);
        }

    }
}
