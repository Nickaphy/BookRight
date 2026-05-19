// Aggregate Root
// Responsible for creating and managing bookings

// Rules:
// - Cannot overlap bookings for the same practitioner
// - Must respect clinic capacity
// - Must calculate final price
// - Must use best discount



// Booking is the main aggregate root
// Responsible for managing appointment creation and validation

// Will later contain:
// - TimeRange
// - Customer reference
// - Practitioner reference
// - Clinic reference
// - BookingLines
// - Price calculation
// - Status


using BookRight.Domain.Common;
using BookRight.Domain.ValueObjects;

namespace BookRight.Domain.Entities.Bookings;

public class Booking : AggregateRoot
{
    public BookingStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public PriceCalculation PriceCalculation { get; private set; }
    public bool IsTeam { get; private set; }
    public int AmountParticipants { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid PractitionerId { get; private set; }
    public Guid ClinicId { get; private set; }
    public Guid TreatmentTypeId { get; private set; }

    private Booking()
    {
        // Required by EF Core
    }

    private Booking(
        Guid customerId,
        Guid practitionerId,
        Guid clinicId,
        Guid treatmentTypeId,
        TimeRange timeRange,
        PriceCalculation priceCalculation,
        bool isTeam,
        int amountParticipants)
    {
        if (timeRange is null)
            throw new ArgumentNullException(nameof(timeRange));

        if (priceCalculation is null)
            throw new ArgumentNullException(nameof(priceCalculation));

        if (amountParticipants < 1)
            throw new ArgumentException("Amount of participants must be at least 1.", nameof(amountParticipants));

        CustomerId = customerId;
        PractitionerId = practitionerId;
        ClinicId = clinicId;
        TreatmentTypeId = treatmentTypeId;
        TimeRange = timeRange;
        PriceCalculation = priceCalculation;
        IsTeam = isTeam;
        AmountParticipants = amountParticipants;

        Status = BookingStatus.Created;
        CreatedDate = DateTime.UtcNow;
    }

    public static Booking Create(
        Guid customerId,
        Guid practitionerId,
        Guid clinicId,
        Guid treatmentTypeId,
        TimeRange timeRange,
        PriceCalculation priceCalculation)
    {
        return new Booking(
            customerId,
            practitionerId,
            clinicId,
            treatmentTypeId,
            timeRange,
            priceCalculation,
            isTeam: false,
            amountParticipants: 1);
    }

    // Cancels the booking.
    // A completed booking cannot be cancelled.
    public void Cancel()
    {
        if (Status == BookingStatus.Completed)
        {
            throw new InvalidOperationException(
                "A completed booking cannot be cancelled.");
        }

        if (Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "The booking is already cancelled.");
        }

        Status = BookingStatus.Cancelled;
    }


    // Marks the booking as completed.
    // Cancelled or no-show bookings cannot be completed.
    public void Complete()
    {
        if (Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException(
                "A cancelled booking cannot be completed.");
        }

        if (Status == BookingStatus.NoShow)
        {
            throw new InvalidOperationException(
                "A no-show booking cannot be completed.");
        }

        if (Status == BookingStatus.Completed)
        {
            throw new InvalidOperationException(
                "The booking is already completed.");
        }

        Status = BookingStatus.Completed;
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

        Status = BookingStatus.NoShow;
    }


    // Future business rules:
    //
    // - Combined treatments
    // - Team bookings
    // - Booking pause workflow
    // - Favorite practitioner support
    // - Advanced cancellation policies
    // - Rescheduling rules
    //
    // Cross-aggregate validation should remain
    // outside the Booking aggregate.
}