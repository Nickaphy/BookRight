    // Represents a treatment type

// Rules:
// - Has duration and base price
// - Requires specific authorization
// - Some treatments can be combined

using BookRight.Domain.Common;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;

namespace BookRight.Domain.Entities.Treatments;

public class TreatmentType : AggregateRoot
{
    public string Name { get; private set; }
    public int DurationMinutes { get; private set; } // in minutes
    public Money BasePrice { get; private set; }
    public AuthorizationType NeedsAuthorisation { get; private set; } //Lucas ændret 09/05
    public int MaxParticipants { get; private set; }


    private TreatmentType() { }

    public TreatmentType(
        string name,
        int duration,
        Money basePrice,
        AuthorizationType needsAuthorisation, //Lucas ændret 09/05
        int maxParticipants
        )
    {
        if ( name == null ) 
            throw new ArgumentException( "Must choose a treatmenttype" );
        if (duration <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(duration));
            
       if (string.IsNullOrWhiteSpace(name))
       
        if (maxParticipants <= 0)
            throw new ArgumentException("Max participants must be greater than 1.", nameof(maxParticipants));

        Name = name;
        DurationMinutes = duration;
        BasePrice = basePrice;  
        NeedsAuthorisation = needsAuthorisation;
        MaxParticipants = maxParticipants;
    }
}
