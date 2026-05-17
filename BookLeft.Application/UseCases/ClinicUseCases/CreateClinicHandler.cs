using BookRight.Application.Repositories;
using BookRight.Domain.Entities.Clinics;
using BookRight.Facade.Commands.Clinic;
using BookRight.Facade.Dtos.ClinicCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCases.ClinicUseCases
{
    public class CreateClinicHandler : ICreateClinic
    {
        private readonly IClinicRepository _clinicRepository;  
        public CreateClinicHandler(IClinicRepository clinicRepository)
        {
            _clinicRepository = clinicRepository;
        }

        public async Task HandleAsync(CreateClinicRequest request, CancellationToken cancellationToken = default)
        {
            var openingHours = request.OpeningHours
                .Select(oh => new ClinicOpeningHour(oh.WeekDay, oh.OpeningTime, oh.ClosingTime))
                .ToArray();
            
            var clinic = Clinic.Create(
                request.Name,
                request.AmountTreatmentRooms,
                request.Street,
                request.City,
                request.Zipcode,
                openingHours
                );


            await _clinicRepository.AddAsync(clinic, cancellationToken);
        }
    }
}
