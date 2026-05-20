using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases
{
    public class UpdatePractitionerHandler : IUpdatePractitioner
    {
        private readonly IPractitionerRepository _practitionerRepository;

        public UpdatePractitionerHandler(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public async Task HandleAsync(UpdatePractitionerCommand command, CancellationToken cancellationToken = default)
        {
            var practitioner = await _practitionerRepository.GetByIdAsync(command.PractitionerId, cancellationToken);

            if (practitioner == null)
            {
                throw new UseCaseException("Practitioner not found.");
            }

            if (!Enum.TryParse<AuthorizationType>(command.AuthorizationType, out var authorizationType))
            {
                throw new UseCaseException("Invalid authorization type.");
            }

            practitioner.UpdateName(command.Name);
            practitioner.UpdateEmail(command.Email);
            practitioner.UpdatePhoneNumber(command.PhoneNumber);
            practitioner.UpdateAuthorization(command.AuthorizationCode, authorizationType);

            await _practitionerRepository.UpdateAsync(practitioner, cancellationToken);
            await _practitionerRepository.SaveAsync(practitioner, cancellationToken);
        }
    }
}
