namespace BookRight.Domain.ValueObjects;

public class Money
{
    public const string Currency = "DKK";
    public decimal Amount { get; private set; }

    private Money() { }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    // Two Money objects are equal if they hold the same amount
    // Overrides default C# reference comparison
    public override bool Equals(object? obj) =>
        obj is Money other && Amount == other.Amount;

    // Required whenever Equals is overridden
    // Ensures equal objects produce the same hash code
    // Used by dictionaries and hash sets internally
    public override int GetHashCode() => Amount.GetHashCode();

    // Prints the amount followed by currency e.g. "395 DKK"
    public override string ToString() => $"{Amount} {Currency}";
}
