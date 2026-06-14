// Aggregate Root
// Responsible for creating and managing bookings

// Rules:
// - Cannot overlap bookings for the same practitioner
// - Must respect clinic capacity
// - Must calculate final price
// - Must use best discount



// Booking is the main aggregate root
// Responsible for managing appointment creation and validation

using BookRight.Domain.Common;
using BookRight.Domain.Entities.Treatments;
using BookRight.Domain.Enums;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;
namespace BookRight.Domain.Entities.Bookings;

public class Booking : AggregateRoot
{
    public Money BasePrice { get; private set; }
    public Money FinalPrice { get; private set; }

    // Stores which discount type won
    // during booking price calculation.
    public DiscountType DiscountType { get; private set; }

    public BookingStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid PractitionerId { get; private set; }
    public Guid ClinicId { get; private set; }
    public Guid TreatmentTypeId { get; private set; }
    public string? AppliedDiscount { get; private set; }
    public bool IsTeam { get; private set; }



    private Booking()
    {
        // Required by EF Core
    }

    public Booking(
        Guid customerId,
        Guid practitionerId,
        Guid clinicId,
        Guid treatmentTypeId,
        TimeRange timeRange,
        Money basePrice,
        bool isTeam)
    {
        if (timeRange is null)
            throw new ArgumentNullException(nameof(timeRange));


        
        if (timeRange.Start < DateTime.Now)
            throw new DomainException("A booking cannot be placed in the past.");

        CustomerId = customerId;
        PractitionerId = practitionerId;
        ClinicId = clinicId;
        TreatmentTypeId = treatmentTypeId;
        TimeRange = timeRange;
        BasePrice = basePrice;
        IsTeam = isTeam;

        Status = BookingStatus.Created;
        CreatedDate = DateTime.UtcNow;
    }

    public static Booking Create(
        Guid customerId,
        Guid practitionerId,
        Guid clinicId,
        Guid treatmentTypeId,
        TimeRange timeRange,
        Money basePrice,
        bool isTeam)
    {
        var booking = new Booking(
            customerId,
            practitionerId,
            clinicId,
            treatmentTypeId,
            timeRange,
            basePrice,
            isTeam);
        return booking;
    }

    // Rebuilds a Booking from existing persisted data without re-running
    // business rules. Use only in DataSeeder and repositories.
    public static Booking Reconstitute(
        Guid customerId, Guid practitionerId, Guid clinicId,
        Guid treatmentTypeId, TimeRange timeRange, Money basePrice,
        BookingStatus status,
        bool isTeam)
    {
        var b = new Booking();
        b.CustomerId = customerId;
        b.PractitionerId = practitionerId;
        b.ClinicId = clinicId;
        b.TreatmentTypeId = treatmentTypeId;
        b.TimeRange = timeRange;
        b.BasePrice = basePrice;
        b.FinalPrice = basePrice;
        b.Status = status;
        b.CreatedDate = timeRange.Start;
        b.IsTeam = isTeam;
        return b;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled.");

        Status = BookingStatus.Cancelled;
    }

    public void SetFinalPrice(Money finalPrice, DiscountType discountType)
    {
        if (finalPrice is null)
            throw new DomainException("Final price cant be null");

        if (finalPrice.Amount > BasePrice.Amount)
            throw new DomainException("Final price cannot be higher than base price.");

        FinalPrice = finalPrice;
        DiscountType = discountType;
    }

    // Marks the booking as no-show.
    // Completed or cancelled bookings cannot become no-show.
    public void MarkNoShow()
    {
        if (Status == BookingStatus.Completed)
        {
            throw new InvalidOperationException(
                "A completed booking cannot be marked as no-show.");
        }

        if (Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "A cancelled booking cannot be marked as no-show.");
        }

        if (Status == BookingStatus.NoShow)
        {
            throw new InvalidOperationException(
                "The booking is already marked as no-show.");
        }
        if (Status == BookingStatus.NotHandled)
        {
            throw new InvalidOperationException(
                "The booking is already marked as no-show.");
        }

        Status = BookingStatus.NoShow;
    }

    public void Complete()
    {
        if (Status == BookingStatus.Completed)
            throw new DomainException("Booking is already completed.");

        if (Status == BookingStatus.Cancelled)
            throw new DomainException("A cancelled booking cannot be completed.");

        if (Status == BookingStatus.NoShow)
            throw new DomainException("A no-show booking cannot be completed.");
        
        if (FinalPrice == null)
            throw new DomainException("Cannot complete a booking before a final price has been set.");

        Status = BookingStatus.Completed;

        

        AddDomainEvent(new BookingCompletedEvent(CustomerId, FinalPrice.Amount));
    }


   

}