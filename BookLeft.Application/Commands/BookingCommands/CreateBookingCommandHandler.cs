

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
using BookRight.Application.Services;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Entities.Bookings;
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
    private readonly IBookingConflictChecker _bookingConflictChecker;
    private readonly IDiscountService _discountService;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPractitionerRepository practitionerRepository,
        //ICampaignRepository campaignRepository,
        IClinicRepository clinicRepository,
        ITreatmentTypeRepository treatmentTypeRepository,
        IBookingConflictChecker bookingConflictChecker,
        IDiscountService discountservice) 
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _practitionerRepository = practitionerRepository;
        //_campaignRepository = campaignRepository;
        _clinicRepository = clinicRepository;
        _treatmentTypeRepository = treatmentTypeRepository;
        _bookingConflictChecker = bookingConflictChecker;
        _discountService = discountservice;
    }

    public async Task<Guid> CreateBookingAsync(
    CreateBookingRequest request,
    CancellationToken cancellationToken = default)
    {
        // ====================
        // Load aggregates
        // ====================

        var customer = await _customerRepository.GetCustomerByIdAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer was not found.");
        }

        var practitioner = await _practitionerRepository.GetByIdAsync(
            request.PractitionerId,
            cancellationToken);

        if (practitioner is null)
        {
            throw new InvalidOperationException("Practitioner was not found.");
        }

        var clinic = await _clinicRepository.GetByIdAsync(
            request.ClinicId,
            cancellationToken);

        if (clinic is null)
        {
            throw new InvalidOperationException("Clinic was not found.");
        }

        var treatmentType = await _treatmentTypeRepository.GetByIdAsync(
            request.TreatmentTypeId,
            cancellationToken);

        if (treatmentType is null)
        {
            throw new InvalidOperationException("Treatment type was not found.");
        }

        // ====================
        // Business rule validation
        // ====================

        // Verify that the practitioner is allowed
        // to perform the selected treatment.
        practitioner.HasAuthorizationForTreatment(
            treatmentType.NeedsAuthorisation);

        var endTime = request.StartTime.AddMinutes(
            treatmentType.DurationMinutes);

        var timeRange = new TimeRange(
            request.StartTime,
            endTime);

        await _bookingConflictChecker
    .EnsurePractitionerAvailabilityAsync(
        request.PractitionerId,
        timeRange,
        cancellationToken);

        await _bookingConflictChecker
            .EnsureClinicAvailabilityAsync(
                request.ClinicId,
                timeRange,
                cancellationToken);

        // ====================
        // Create value objects
        // ====================

        /*var basePrice = treatmentType.BasePrice;  //Mudder luder lucas har rettet

        var discountResult = await _discountService.GetBestDiscountAsync(
            basePrice,
            cancellationToken);

        var finalPrice = new Money(basePrice.Amount - discountResult.BestDiscount);*/

        // ====================
        // Create aggregate
        // ====================

        var basePrice = treatmentType.BasePrice;

        var booking = Booking.Create(
            request.CustomerId,
            request.PractitionerId,
            request.ClinicId,
            request.TreatmentTypeId,
            timeRange,
            basePrice);

        //Lucas har rettet
        var bestDiscount = await _discountService.GetBestDiscountAsync(booking, cancellationToken);
        var finalPrice = new Money(basePrice.Amount - bestDiscount);  

        booking.SetFinalPrice(finalPrice);


        // ====================
        // Persist aggregate
        // ====================

        await _bookingRepository.AddAsync(
            booking,
            cancellationToken);

        await _bookingRepository.SaveChangesAsync(
            cancellationToken);

        return booking.Id;
    }
}
