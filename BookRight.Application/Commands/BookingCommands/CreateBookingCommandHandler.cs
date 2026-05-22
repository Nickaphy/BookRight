

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
using BookRight.Application.UseCases.Services.PriceCalculator;
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
    private readonly IPriceCalculator _priceCalculator;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPractitionerRepository practitionerRepository,
        //ICampaignRepository campaignRepository,
        IClinicRepository clinicRepository,
        ITreatmentTypeRepository treatmentTypeRepository,
        IBookingConflictChecker bookingConflictChecker,
        IDiscountService discountservice,
        IPriceCalculator priceCalculator)

    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _practitionerRepository = practitionerRepository;
        //_campaignRepository = campaignRepository;
        _clinicRepository = clinicRepository;
        _treatmentTypeRepository = treatmentTypeRepository;
        _bookingConflictChecker = bookingConflictChecker;
        _discountService = discountservice;
        _priceCalculator = priceCalculator;
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
            throw new InvalidOperationException(
                "Customer was not found.");
        }

        var practitioner = await _practitionerRepository.GetByIdAsync(
            request.PractitionerId,
            cancellationToken);

        if (practitioner is null)
        {
            throw new InvalidOperationException(
                "Practitioner was not found.");
        }

        var clinic = await _clinicRepository.GetByIdAsync(
            request.ClinicId,
            cancellationToken);

        if (clinic is null)
        {
            throw new InvalidOperationException(
                "Clinic was not found.");
        }

        var treatmentType = await _treatmentTypeRepository.GetByIdAsync(
            request.TreatmentTypeId,
            cancellationToken);

        if (treatmentType is null)
        {
            throw new InvalidOperationException(
                "Treatment type was not found.");
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

        // Ensure practitioner availability.
        await _bookingConflictChecker
            .EnsurePractitionerAvailabilityAsync(
                request.PractitionerId,
                timeRange,
                cancellationToken);

        // Ensure clinic availability.
        await _bookingConflictChecker
            .EnsureClinicAvailabilityAsync(
                request.ClinicId,
                timeRange,
                cancellationToken);

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

        // ====================
        // Build pricing context
        // ====================

        // Determine whether the booking occurs
        // during the customer's birthday month.
        var isBirthdayMonth =
            request.StartTime.Month ==
            customer.DateOfBirth.Month;

        // Check whether the customer has already used
        // the birthday discount during the current year.
        var hasUsedBirthdayDiscountThisYear =
            await _bookingRepository
                .HasUsedBirthdayDiscountAsync(
                    customer.Id,
                    request.StartTime.Year,
                    cancellationToken);

        // Build pricing context used by discount strategies.
        var pricingContext = new BookingPricingContext
        {
            Booking = booking,

            Customer = customer,

            IsBirthdayMonth = isBirthdayMonth,

            HasUsedBirthdayDiscountThisYear =
                hasUsedBirthdayDiscountThisYear,

            IsEveningOrWeekend = false,

            CampaignDiscountPercent = null
        };

        // ====================
        // Calculate discounts
        // ====================

        // Execute all discount strategies
        // and select the best available discount.
        var discountResult =
            await _discountService.GetBestDiscountAsync(
                pricingContext,
                cancellationToken);

        // Calculate final booking price.
        var (finalPrice, winningDiscountType) =
            await _priceCalculator.CalculateFinalPriceAsync(
                pricingContext,
                cancellationToken);

        // Store final price on aggregate.
        booking.SetFinalPrice(
            finalPrice,
            winningDiscountType);

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
};
