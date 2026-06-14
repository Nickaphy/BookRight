// Value Object
// Represents Start and End time
// Used for overlap validation

using BookRight.Domain.Common;

namespace BookRight.Domain.ValueObjects;

public sealed class TimeRange : ValueObject
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }


    public TimeSpan Duration => End - Start;

    private TimeRange()
    {    }
   

    // Her vælger vi IKKE at bruge "record" da det vil konflikte med vores "private set",
    // så i tilfældet her arbejder vi ud fra "code-first".
    public TimeRange(DateTime start, DateTime end)
    {
        if (end <= start)
        {
            throw new ArgumentException("Start time must be before end time.");
        }

        Start = start;
        End = end;

    }
    protected override IEnumerable<object> GetEqualityComponents()
       {
           yield return Start;
           yield return End;
       }
    //denne bool gør at der kan være "back-to-back" tider. - Den sørger for en tid ikke kan starte mens en tid er i gang.
    public bool Overlaps(TimeRange other)
    {
        return Start < other.End && End > other.Start;
    }

}
