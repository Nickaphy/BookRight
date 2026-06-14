using BookRight.Facade.Commands.Clinic;
using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Facade.Dtos.CommandDto.ClinicCommand;

namespace BookRight.Application.UseCases.ClinicUseCases
{
    public class DeleteClinicHandler : IDeleteClinic
    {
        private readonly IClinicRepository _clinicRepository;

        public DeleteClinicHandler(IClinicRepository clinicRepository)
        {
            _clinicRepository = clinicRepository;
        }

        public async Task HandleAsync(DeleteClinicRequest request, CancellationToken cancellationToken = default)
        {
            
            var clinic = await _clinicRepository.GetByIdAsync(request.Id, cancellationToken);

            if (clinic == null)
            {
                throw new UseCaseException("Clinic not found");
            }

            await _clinicRepository.DeleteAsync(clinic, cancellationToken);
            await _clinicRepository.SaveAsync(clinic, cancellationToken);
        }
    }
}
