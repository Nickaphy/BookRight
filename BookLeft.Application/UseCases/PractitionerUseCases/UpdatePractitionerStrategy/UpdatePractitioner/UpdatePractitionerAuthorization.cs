using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Application.UseCaseExceptions;
using BookRight.Facade.Dtos.PractitionerCommand;
using BookRight.Domain.Entities.Practitioners;

namespace BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy.UpdatePractitioner
{
    public class UpdatePractitionerAuthorization : PractitionerUpdateStrategyBase
    {
        public UpdatePractitionerAuthorization(IPractitionerRepository practitionerRepository) : base(practitionerRepository) { }
    
        public override async Task UpdateAsync(Practitioner entity, object command)
        {
            if (entity == null)
            {
                throw new UseCaseException("Practitioner not found.");
            }
            var updateCommand = command as UpdatePractitionerAuthorizationCommand;
            if (updateCommand == null)
            {
                throw new UseCaseException("Invalid command type.");
            }
            if (!Enum.TryParse<AuthorizationType>(updateCommand.AuthorizationType, out var authorizationType))
            {
                throw new UseCaseException("Invalid authorization type.");
            }

            entity.UpdateAuthorization(updateCommand.AuthorizationCode, authorizationType);
            await _practitionerRepository.UpdateAsync(entity);
        }
    }
}
