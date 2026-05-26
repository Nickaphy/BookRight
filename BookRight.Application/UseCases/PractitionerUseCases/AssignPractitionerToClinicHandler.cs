using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Commands.Practitioner;
using BookRight.Facade.Dtos.PractitionerCommand;

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
                throw new UseCaseException($"Practitioner with ID {command.PractitionerId} not found.");
            practitioner.AssignToClinic(command.ClinicId, command.Date);
            await _practitionerRepository.UpdateAsync(practitioner, cancellationToken);
            await _practitionerRepository.SaveAsync(practitioner, cancellationToken); 
        }
    }
}
