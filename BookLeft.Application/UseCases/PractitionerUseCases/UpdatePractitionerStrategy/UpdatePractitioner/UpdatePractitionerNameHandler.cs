using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Dtos.PractitionerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy.UpdatePractitioner
{
    internal class UpdatePractitionerNameHandler : PractitionerUpdateStrategyBase
    {
        public UpdatePractitionerNameHandler(IPractitionerRepository practitionerRepository) : base(practitionerRepository) {}
        public override async Task UpdateAsync(Practitioner entity, object command)
        {
            if (entity == null)
            {
                throw new UseCaseException("Practitioner not found.");
            }

            var updateCommand = command as UpdatePractitionerNameCommand;
            if (updateCommand == null)
            {
                throw new UseCaseException("Invalid command type.");
            }

            //her går vi forbi invarianterne i Practitioner-klassen, og opdaterer emailen direkte.
            //Det er vigtigt at sikre, at valideringen i Practitioner-klassen stadig bliver overholdt, når vi gør dette.
            entity.UpdateName(updateCommand.Name);

            await _practitionerRepository.UpdateAsync(entity);
        }
    }
}
