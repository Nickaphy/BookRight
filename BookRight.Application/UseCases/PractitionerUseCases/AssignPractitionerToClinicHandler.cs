using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Domain.Entities.Clinics;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.CommandDto.PractitionerCommand;

namespace BookRight.Application.UseCases.PractitionerUseCases
{
    public class AssignPractitionerToClinicHandler : IAssignPractitionerToClinic
    {
        private readonly IPractitionerRepository _practitionerRepository;
        private readonly IClinicRepository _clinicRepository;

        public AssignPractitionerToClinicHandler(IPractitionerRepository practitionerRepository, IClinicRepository clinicRepository)
        {
            _practitionerRepository = practitionerRepository;
            _clinicRepository = clinicRepository;
        }

        public async Task HandleAsync(AssignPractitionerToClinicCommand command, CancellationToken cancellationToken = default)
        {
            var practitioner = await _practitionerRepository.GetByIdAsync(command.PractitionerId, cancellationToken);
            if (practitioner == null)
                throw new UseCaseException($"Practitioner with ID {command.PractitionerId} not found.");


            var clinic = await _clinicRepository.GetByIdAsync(command.ClinicId, cancellationToken);
            if (clinic == null)
                throw new UseCaseException($"Clinic with ID {command.ClinicId} not found.");


            var assignedCount = await _practitionerRepository
            .CountPractitionersByClinicAndDateAsync(command.ClinicId, command.Date, cancellationToken);

            if (assignedCount >= clinic.AmountTreatmentRooms)
                throw new UseCaseException($"Clinic has no available rooms on {command.Date:d}.");

            practitioner.AssignToClinic(command.ClinicId, command.Date);
            await _practitionerRepository.UpdateAsync(practitioner, cancellationToken);
            await _practitionerRepository.SaveAsync(practitioner, cancellationToken); 
        }
    }
}
