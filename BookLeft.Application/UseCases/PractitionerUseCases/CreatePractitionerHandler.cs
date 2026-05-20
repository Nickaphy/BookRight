using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases
{
    public class CreatePractitionerHandler : ICreatePractitioner
    {
        private readonly IPractitionerRepository _practitionerRepository;


        public CreatePractitionerHandler(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public async Task HandleAsync(CreatePractitionerCommand command, CancellationToken cancellationToken = default)
        {
            var authorizationType = Enum.Parse<AuthorizationType>(command.AuthorizationType);

            var practitioner = Practitioner.Create(
            command.Name,
            command.Email,
            command.PhoneNumber,
            command.AuthorizationCode,
            authorizationType
);
            await _practitionerRepository.AddAsync(practitioner, cancellationToken);
            await _practitionerRepository.SaveAsync(practitioner, cancellationToken);
        }
    }
}
