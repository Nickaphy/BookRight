using BookRight.Application.Repositories;
using BookRight.Domain.DomainServices.BookingConfictChecker;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Commands.Booking;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;

namespace BookRight.Application.UseCases.BookingCommands;

public sealed class CreateBookingCommandHandler : ICreateBookingUseCase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPractitionerRepository _practitionerRepository;
    private readonly IClinicRepository _clinicRepository;
    private readonly ITreatmentTypeRepository _treatmentTypeRepository;
    private readonly IBookingConflictChecker _bookingConflictChecker;
    private readonly IPriceCalculator _priceCalculator;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPractitionerRepository practitionerRepository,
        IClinicRepository clinicRepository,
        ITreatmentTypeRepository treatmentTypeRepository,
        IBookingConflictChecker bookingConflictChecker,
        IPriceCalculator priceCalculator)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _practitionerRepository = practitionerRepository;
        _clinicRepository = clinicRepository;
        _treatmentTypeRepository = treatmentTypeRepository;
        _bookingConflictChecker = bookingConflictChecker;
        _priceCalculator = priceCalculator;
    }

    public async Task<Guid> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default)
    {
      
        // Load aggregates
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
        
        // Business rule validation
        
        // Verify that the practitioner is allowed
        // to perform the selected treatment.
        practitioner.HasAuthorizationForTreatment(
            treatmentType.NeedsAuthorisation);

        var endTime = request.StartTime.AddMinutes(
            treatmentType.DurationMinutes);

        var timeRange = new TimeRange(
            request.StartTime,
            endTime);

        var existingForCustomer = await _bookingRepository.GetBookingsByCustomerIdAsync(
            request.CustomerId, cancellationToken);

        var existingForPractitioner = await _bookingRepository.GetBookingsByPractitionerIdAsync(
            request.PractitionerId, cancellationToken);

        // Create aggregate

        // Create a fresh Money instance rather than passing treatmentType.BasePrice
        // directly. EF Core tracks owned entities by instance — reusing the same
        // Money object that belongs to TreatmentType causes an owned-entity key
        // conflict when SaveChanges tries to associate it with Booking.BasePrice.
        var basePrice = new Money(treatmentType.BasePrice.Amount);

        _bookingConflictChecker.ValidateWithoutOverlapping(
            timeRange,
            treatmentType.MaxParticipants,
            existingForCustomer,
            existingForPractitioner);

        var booking = Booking.Create(
            request.CustomerId,
            request.PractitionerId,
            request.ClinicId,
            request.TreatmentTypeId,
            timeRange,
            basePrice,
            request.IsTeam);
            
        
        // Build pricing context

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
                    customer.DateOfBirth.Month,
                    cancellationToken);

        // Build pricing context used by the price calculator
        // and discount strategies.
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
        
        // Calculate final price

        // PriceCalculator owns the pricing flow.
        // It calls DiscountService internally and returns
        // both final price and winning discount type.
        var (finalPrice, winningDiscountType) =
            await _priceCalculator.CalculateFinalPriceAsync(
                pricingContext,
                cancellationToken);

        // Store final price and discount type on the aggregate.
        booking.SetFinalPrice(
            finalPrice,
            winningDiscountType);

        // Persist aggregate

        await _bookingRepository.AddAsync(
            booking,
            cancellationToken);

        await _bookingRepository.SaveChangesAsync(
            cancellationToken);

        return booking.Id;
    }
}