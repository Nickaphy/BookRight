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





/*using BookRight.Domain.Common;

namespace BookRight.Domain.Bookings;

public class Booking : AggregateRoot
{
    public BookingStatus status { get; private set; }
    public DateTime createdDate { get; private set; }
    public TimeSpan timeSpan { get; private set; }
    public PriceCalculation priceCalculation { get; private set; }
    public bool isTeam { get; private set; }
    public int amountParticipants { get; private set; }
    public Guid customerId { get; private set; }
    public Guid practitionerId { get; private set; }
    public Guid clinicId { get; private set; }
    public TreatmentType treatmentType { get; private set; }

    private Booking() { }

    public Booking(
        BookingStatus status,
        DateTime createdDate,
        TimeSpan timeSpan,
        PriceCalculation priceCalculation,
        bool isTeam,
        int amountParticipants,
        Guid customerId,
        Guid practitionerId,
        Guid clinicId,
        TreatmentType treatmentType
        )
    {
        if (amountParticipants < 1)
            throw new ArgumentException("Amount of participants must be at least 1.", nameof(amountParticipants));

        this.status = status;
        this.createdDate = createdDate;
        this.timeSpan = timeSpan;
        this.priceCalculation = priceCalculation;
        this.isTeam = isTeam;
        this.amountParticipants = amountParticipants;
        this.customerId = customerId;
        this.practitionerId = practitionerId;
        this.clinicId = clinicId;
        this.treatmentType = treatmentType;
    }
}
*/

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
}
