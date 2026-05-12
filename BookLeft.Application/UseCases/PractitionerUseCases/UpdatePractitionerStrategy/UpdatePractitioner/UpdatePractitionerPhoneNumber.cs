using BookRight.Application.UseCaseExceptions;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.PractitionerCommand;

namespace BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy.UpdatePractitioner
{
    public class UpdatePractitionerPhoneNumber : PractitionerUpdateStrategyBase
    {


        public UpdatePractitionerPhoneNumber(IPractitionerRepository practitionerRepository) : base(practitionerRepository) { }

        public override async Task UpdateAsync(Practitioner entity, object command)
        {
            if (entity == null)
            {
                throw new UseCaseException("Practitioner not found.");
            }

            var updateCommand = command as UpdatePractitionerPhoneNumberCommand;
            if (updateCommand == null)
            {
                throw new UseCaseException("Invalid command type.");
            }

            entity.UpdatePhoneNumber(updateCommand.PhoneNumber);
            await _practitionerRepository.UpdateAsync(entity);
        }
    }
}
