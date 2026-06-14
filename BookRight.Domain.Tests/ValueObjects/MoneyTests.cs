namespace BookRight.Domain.Tests.ValueObjects;

using BookRight.Domain.ValueObjects;
using BookRight.Domain.Exceptions;

// Theory: being used to run multiple tests with different input data made possible by the inline data attribute.
// Fact: a single test with single input data.

public class MoneyTests
{
    // Ensures a valid amount is stored and readable after construction
    [Fact]
    public void Constructor_ValidAmount_StoresAmountCorrectly()
    {
        var money = new Money(395m);
        Assert.Equal(395m, money.Amount);
    }

    // Zero is a valid amount — e.g. a free treatment or no supplement
    [Fact]
    public void Constructor_ZeroAmount_IsValid()
    {
        var money = new Money(0m);
        Assert.Equal(0m, money.Amount);
    }

    // Negative amounts are never valid — Money always represents a real price
    [Fact]
    public void Constructor_NegativeAmount_ThrowsArgumentException()
    {
        Assert.Throws<DomainException>(() => new Money(-1m));
    }

    // Two Money objects with the same amount should be considered equal
    // This works because we overrode Equals to compare by Amount value
    [Fact]
    public void Equals_SameAmount_ReturnsTrue()
    {
        var a = new Money(100m);
        var b = new Money(100m);
        Assert.Equal(a, b);
    }

    // Two Money objects with different amounts should not be equal
    [Fact]
    public void Equals_DifferentAmount_ReturnsFalse()
    {
        var a = new Money(100m);
        var b = new Money(200m);
        Assert.NotEqual(a, b);
    }

    // ToString should print the amount followed by the currency e.g. "395 DKK"
    [Fact]
    public void ToString_ReturnsAmountFollowedByCurrency()
    {
        var money = new Money(395m);
        Assert.Equal("395 DKK", money.ToString());
    }
}
