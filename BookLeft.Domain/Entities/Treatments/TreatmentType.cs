namespace BookRight.Domain.Entities.Treatments;
    
using BookRight.Domain.Common;
using BookRight.Domain.ValueObjects;

public class TreatmentType : AggregateRoot
{
    public string Name { get; private set; }
    public int Duration { get; private set; } // in minutes
    public Money BasePrice { get; private set; }
    public AuthorizationType NeedsAuthorisation { get; private set; }
    public int MaxParticipants { get; private set; }


    private TreatmentType() { }

    public TreatmentType(
        string name,
        int duration,
        Money basePrice,
        AuthorizationType needsAuthorisation,
        int maxParticipants
        )
    {
        if ( name == null ) 
            throw new ArgumentException( "Must choose a treatmenttype" );
        if (duration <= 0)
            throw new ArgumentException("Duration must be greater than 0.", nameof(duration));
        if (maxParticipants <= 0)
            throw new ArgumentException("Max participants must be greater than 1.", nameof(maxParticipants));

        Name = name;
        BasePrice = basePrice;
        Duration = duration;
        NeedsAuthorisation = needsAuthorisation;
        MaxParticipants = maxParticipants;
    }
}