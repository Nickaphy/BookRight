// Represents a treatment type

// Rules:
// - Has duration and base price
// - Requires specific authorization
// - Some treatments can be combined


/*
using BookRight.Domain.Common;
using BookRight.Domain.ValueObjects;

public class TreatmentType : AggregateRoot
{
    public string Name { get; private set; }
    public int Duration { get; private set; } // in minutes
    public decimal BasePrice { get; private set; }
    public AuthorisationType NeedsAuthorisation { get; private set; }
    public int MaxParticipants { get; private set; }


    private TreatmentType() { }

    public TreatmentType(
        string name,
        int duration,
        decimal basePrice,
        AuthorisationType needsAuthorisation,
        int maxParticipants
        )
    {
        if ( name == null ) 
            throw new ArgumentException( "Must choose a treatmenttype" );
        if (duration <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(duration));
        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));
        if (maxParticipants <= 1)
            throw new ArgumentException("Max participants must be greater than 1.", nameof(maxParticipants));

        Name = name;
        Duration = duration;
        BasePrice = basePrice;
        NeedsAuthorisation = needsAuthorisation;
        MaxParticipants = maxParticipants;
    }
}
*/

using BookRight.Domain.Common;
using BookRight.Domain.Enums;

namespace BookRight.Domain.Entities.Treatments;

public class TreatmentType : AggregateRoot
{
    public string Name { get; private set; }
    public int DurationMinutes { get; private set; }
    public decimal BasePrice { get; private set; }
    public AuthorizationType NeedsAuthorization { get; private set; }
    public int MaxParticipants { get; private set; }

    private TreatmentType()
    {
        // Required by EF Core
    }

    public TreatmentType(
        string name,
        int durationMinutes,
        decimal basePrice,
        AuthorizationType needsAuthorization,
        int maxParticipants)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Must choose a treatment type.", nameof(name));

        if (durationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(durationMinutes));

        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative.", nameof(basePrice));

        if (maxParticipants < 1)
            throw new ArgumentException("Max participants must be at least 1.", nameof(maxParticipants));

        Name = name;
        DurationMinutes = durationMinutes;
        BasePrice = basePrice;
        NeedsAuthorization = needsAuthorization;
        MaxParticipants = maxParticipants;
    }
}