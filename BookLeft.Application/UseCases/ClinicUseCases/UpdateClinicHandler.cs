using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Facade.Commands.Clinic;
using BookRight.Facade.Dtos.ClinicCommand;
using BookRight.Domain.ValueObjects;
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

        public async Task HandleAsync(UpdateClinicRequest request, CancellationToken cancellationToken = default)
        {
            var clinic = await _clinicRepository.GetByIdAsync(request.Id, cancellationToken);
            if (clinic == null)
                throw new UseCaseException("Clinic not found");

            var openingHours = request.OpeningHours
                .Select(oh => new ClinicOpeningHour(oh.WeekDay, oh.OpeningTime, oh.ClosingTime))
                .ToList();

            clinic.UpdateClinic(
                request.Name,
                request.AmountTreatmentRooms,
                request.Street,
                request.City,
                request.Zipcode,
                openingHours
            );

            await _clinicRepository.UpdateAsync(clinic, cancellationToken);
        }
    }
}
