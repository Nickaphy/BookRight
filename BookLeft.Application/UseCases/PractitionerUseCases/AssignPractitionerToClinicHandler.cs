using BookRight.Application.Repositories;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases
{
    public class AssignPractitionerToClinicHandler : IAssignPractitionerToClinic
    {
        private readonly IPractitionerRepository _practitionerRepository;

        public AssignPractitionerToClinicHandler(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public async Task HandleAsync(AssignPractitionerToClinicCommand command, CancellationToken cancellationToken = default)
        {
            var practitioner = await _practitionerRepository.GetByIdAsync(command.PractitionerId, cancellationToken);
            if (practitioner == null)
            {
                throw new Exception($"Practitioner with ID {command.PractitionerId} not found.");
            }
            practitioner.AssignToClinic(command.ClinicId, command.Date);
            await _practitionerRepository.UpdateAsync(practitioner, cancellationToken);
        }
    }
}
