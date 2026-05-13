

// Command handler
// Executes the use case
// Coordinates repositories, domain rules and services
// Does not contain UI code



/*
CreateBooking.razor
↓
IBookingFacade
↓
CreateBookingCommand
↓
CreateBookingCommandHandler
↓
Booking domain model
↓
IBookingRepository
↓
BookingRepository
↓
BookRightDbContext
↓
SQL Server
*/


/*
CreateBookingCommandHandler
→ bruger Domain regler
→ kalder repository
→ SaveChanges()
*/



// COMMAND HANDLER
// Responsible for changing system state

// Flow:
// 1. Validate input
// 2. Check domain rules (overlap, capacity, practitioner availability)
// 3. Calculate price (using discount strategies)
// 4. Create Booking entity
// 5. Save using repository


// Erik´s work


using BookRight.Application.Repositories;
using BookRight.Domain.Bookings;
//using BookRight.Domain.Entities.Bookings; //kaster fejl Lucas Rettet.
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Commands;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos;
using BookRight.Facade.Dtos.BookingCommand;

namespace BookRight.Application.Commands.BookingCommands;

public sealed class CreateBookingCommandHandler : ICreateBookingUseCase //Handleren er nu Usecasen, som har interfacet som refferance
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPractitionerRepository _practitionerRepository;
    //private readonly ICampaignRepository _campaignRepository;
    private readonly IClinicRepository _clinicRepository;
    private readonly ITreatmentTypeRepository _treatmentTypeRepository;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPractitionerRepository practitionerRepository,
        //ICampaignRepository campaignRepository,
        IClinicRepository clinicRepository,
        ITreatmentTypeRepository treatmentTypeRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _practitionerRepository = practitionerRepository;
        //_campaignRepository = campaignRepository;
        _clinicRepository = clinicRepository;
        _treatmentTypeRepository = treatmentTypeRepository;
    }

    public async Task<Guid> CreateBookingAsync( //rettet Lucas - tidligere: HandleAsync
        CreateBookingRequest request,           //med request i stedet for command, da det er det requesten hedder i Facade laget
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer was not found.");
        }

        var practitioner = await _practitionerRepository.GetByIdAsync(request.PractitionerId, cancellationToken);

        if (practitioner is null)
        {
            throw new InvalidOperationException("Practitioner was not found.");
        }

        var clinic = await _clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);

        if (clinic is null)
        {
            throw new InvalidOperationException("Clinic was not found.");
        }

        var treatmentType = await _treatmentTypeRepository.GetByIdAsync(request.TreatmentTypeId, cancellationToken);

        if (treatmentType is null)
        {
            throw new InvalidOperationException("Treatment type was not found.");
        }

        var endTime = request.StartTime.AddMinutes(treatmentType.DurationMinutes);
        var timeRange = new TimeRange(request.StartTime, endTime);

        var practitionerHasOverlap =
            await _bookingRepository.HasOverlappingBookingForPractitionerAsync(
                request.PractitionerId,
                timeRange,
                cancellationToken);

        if (practitionerHasOverlap)
        {
            throw new InvalidOperationException("The practitioner already has a booking in this time range.");
        }

        var clinicHasOverlap =
            await _bookingRepository.HasOverlappingBookingForClinicAsync(
                request.ClinicId,
                timeRange,
                cancellationToken);

        if (clinicHasOverlap)
        {
            throw new InvalidOperationException("The clinic has no available room in this time range.");
        }

        var priceCalculation = PriceCalculation.Create(
            new Money(treatmentType.BasePrice),
            LoyaltyLevel.None,
            isBirthdayMonth: false,
            isEveningOrWeekend: false,
            campaignDiscountPercent: null);

        var booking = Booking.Create(
            request.CustomerId,
            request.PractitionerId,
            request.ClinicId,
            request.TreatmentTypeId,
            timeRange,
            priceCalculation);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
