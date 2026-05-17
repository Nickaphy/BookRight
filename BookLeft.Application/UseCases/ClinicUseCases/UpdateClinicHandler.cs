using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Facade.Commands.Clinic;
using BookRight.Facade.Dtos.ClinicCommand;
using BookRight.Application.UseCaseExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.ClinicUseCases
{
    public class UpdateClinicHandler : IUpdateClinic
    {

        public readonly IClinicRepository _clinicRepository;

        public UpdateClinicHandler(IClinicRepository clinicRepository)
        {
            _clinicRepository = clinicRepository;
        }

        public Task HandleAsync(UpdateClinicRequest request, CancellationToken cancellationToken = default)
        {
            var clinic = _clinicRepository.GetByIdAsync(request.Id, cancellationToken).Result;
            if (clinic == null)
            {
                throw new UseCaseException("Clinic not found");
            }

            // Update clinic properties
            clinic.UpdateClinic(request.Name, request.AmountTreatmentRooms, request.City, request.Zipcode,request.Street);
            

            

            return _clinicRepository.UpdateAsync(clinic, cancellationToken);
        }
    }
}
