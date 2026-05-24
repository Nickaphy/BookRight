// Represents a treatment type
//
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

    // Duration in minutes.
    public int DurationMinutes { get; private set; }

    public Money BasePrice { get; private set; }

    // Which practitioner authorization is required.
    public AuthorizationType NeedsAuthorisation { get; private set; }

    // Maximum allowed participants.
    public int MaxParticipants { get; private set; }

    private TreatmentType()
    {
        // Required by EF Core.
    }

    public TreatmentType(
        string name,
        int duration,
        Money basePrice,
        AuthorizationType needsAuthorisation,
        int maxParticipants)
    {
        // Name is required.
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Must choose a treatment type.",
                nameof(name));

        // Duration must be positive.
        if (duration <= 0)
            throw new ArgumentException(
                "Duration must be greater than 0.",
                nameof(duration));

        // Base price is required.
        if (basePrice is null)
            throw new ArgumentNullException(
                nameof(basePrice));

        // At least one participant must be allowed.
        if (maxParticipants <= 0)
            throw new ArgumentException(
                "Max participants must be at least 1.",
                nameof(maxParticipants));

        Name = name;
        DurationMinutes = duration;
        BasePrice = basePrice;
        NeedsAuthorisation = needsAuthorisation;
        MaxParticipants = maxParticipants;
    }
}