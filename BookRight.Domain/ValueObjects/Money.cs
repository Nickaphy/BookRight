using BookRight.Domain.Common;
using BookRight.Domain.Exceptions;

namespace BookRight.Domain.ValueObjects;

public class Money : ValueObject
{
    public const string Currency = "DKK";
    public decimal Amount { get; init; }

    private Money() { }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative.");
        Amount = amount;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    

    // Prints the amount followed by currency e.g. "395 DKK"
    public override string ToString() => $"{Amount} {Currency}";
}
