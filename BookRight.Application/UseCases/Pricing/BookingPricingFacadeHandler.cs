using BookRight.Application.Repositories;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Dtos.BookingPreview;
using BookRight.Facade.Querries.BookingQuerries;

namespace BookRight.Application.UseCases.Pricing;


// Calculates a price preview by running the real discount strategy
// pipeline against a temporary (non-persisted) booking.
//
// Flow:
//   1. Load the customer and treatment type from the repositories.
//   2. Derive the customer's current loyalty level FRESH from their
//      12-month booking history (same logic as UpdateCustomerLoyaltyLevel
//      handler) so the strategies always see the correct level regardless
//      of whether the stored DB column has been updated yet.
//   3. Build a BookingPricingContext and hand it to IPriceCalculator, which
//      fires all IDiscountStrategy implementations in parallel (Task.WhenAll).
//      Each strategy races through the BestDiscountResult lock gate and
//      overwrites the current best only if its own discount is larger.
//   4. Return a DTO with the base price, winning discount, and final price
//      so the confirmation screen can display them without any pricing
//      logic living in the UI layer.

public sealed class BookingPricingFacadeHandler : IBookingPricingFacade
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITreatmentTypeRepository _treatmentTypeRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPriceCalculator _priceCalculator;

    public BookingPricingFacadeHandler(
        ICustomerRepository customerRepository,
        ITreatmentTypeRepository treatmentTypeRepository,
        IBookingRepository bookingRepository,
        IPriceCalculator priceCalculator)
    {
        _customerRepository = customerRepository;
        _treatmentTypeRepository = treatmentTypeRepository;
        _bookingRepository = bookingRepository;
        _priceCalculator = priceCalculator;
    }

    public async Task<BookingPricePreviewDto> GetPricePreviewAsync(
        Guid customerId,
        Guid treatmentTypeId,
        DateTime startTime,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository
            .GetCustomerByIdAsync(customerId, cancellationToken)
            ?? throw new InvalidOperationException("Customer not found.");

        var treatment = await _treatmentTypeRepository
            .GetByIdAsync(treatmentTypeId, cancellationToken)
            ?? throw new InvalidOperationException("Treatment type not found.");

        // ── Step 1: derive loyalty level fresh from booking history ──────────
        //
        // UpdateCustomerLoyaltyLevelHandler only runs when a booking is
        // marked Completed, so the stored LoyaltyLevel column can lag behind
        // reality. We recalculate it here from the actual 12-month spend total
        // (same query and thresholds the domain uses) and apply it to the
        // in-memory customer entity before building the pricing context.
        // Nothing is persisted — this is purely so the strategies see the
        // correct level when they inspect context.Customer.LoyaltyLevel.

        var totalSpent = await _bookingRepository
            .GetTotalSpentLastYearAsync(customerId, cancellationToken);

        customer.UpdateLoyaltyLevel(totalSpent);

        // ── Step 2: build a temporary booking for the pricing engine ─────────
        var timeRange = new TimeRange(
            startTime,
            startTime.AddMinutes(treatment.DurationMinutes));

        var tempBooking = Booking.Create(
            customerId,
            Guid.Empty,
            Guid.Empty,
            treatmentTypeId,
            timeRange,
            new Money(treatment.BasePrice.Amount));

        // ── Step 3: assemble the pricing context ─────────────────────────────
        var isBirthdayMonth = startTime.Month == customer.DateOfBirth.Month;

        var hasUsedBirthdayDiscount = await _bookingRepository
            .HasUsedBirthdayDiscountAsync(
                customerId,
                startTime.Year,
                customer.DateOfBirth.Month,
                cancellationToken);

        var context = new BookingPricingContext
        {
            Booking = tempBooking,
            Customer = customer,
            IsBirthdayMonth = isBirthdayMonth,
            HasUsedBirthdayDiscountThisYear = hasUsedBirthdayDiscount,
            IsEveningOrWeekend = false,
            CampaignDiscountPercent = null
        };

        // ── Step 4: run all strategies in parallel through the lock gate ──────
        //
        // IPriceCalculator → IDiscountService → IEnumerable<IDiscountStrategy>
        //
        // Each strategy runs as a Task. When it returns, it calls
        // BestDiscountResult.OfferDiscount which acquires a Lock and only
        // writes if the new discount beats the current best. The winning
        // discount type and amount come back once Task.WhenAll completes.

        var (finalPrice, discountType) =
            await _priceCalculator.CalculateFinalPriceAsync(context, cancellationToken);

        var discountAmount = treatment.BasePrice.Amount - finalPrice.Amount;

        var discountLabel = discountType switch
        {
            DiscountType.Bronze => $"Bronze loyalitetsrabat (5 %)",
            DiscountType.Silver => $"Sølv loyalitetsrabat (10 %)",
            DiscountType.Gold => $"Guld loyalitetsrabat (15 %)",
            DiscountType.BirthdayMonth => $"Fødselsmåneds rabat (25 %)",
            DiscountType.Campaign => $"Kampagnerabat",
            _ => "Ingen rabat"
        };

        return new BookingPricePreviewDto(
            treatment.BasePrice.Amount,
            discountAmount,
            finalPrice.Amount,
            discountLabel);
    }
}