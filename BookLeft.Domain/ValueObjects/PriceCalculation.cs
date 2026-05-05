using BookRight.Domain.Enums;

namespace BookRight.Domain.ValueObjects;

public class PriceCalculation
{
    public decimal BasePrice { get; private set; }
    public DiscountType DiscountType { get; private set; }
    public int DiscountPercent { get; private set; }
    public decimal EveningWeekendSupplement { get; private set; }

    //Derived properties
    public decimal DiscountPrice => BasePrice - (BasePrice * DiscountPercent / 100);
    public decimal FinalPrice => DiscountPrice + EveningWeekendSupplement;

    //For EF Core
    private PriceCalculation() { }

    //Factory method, used to create a new PriceCalculation called upon from a booking
    public static PriceCalculation Create(
        decimal basePrice,
        int discountPercent,
        decimal eveningWeekendSupplement,
        DiscountType discountType)
    {
        if (basePrice <= 0)
            throw new ArgumentException("Base price must be greater than 0.", nameof(basePrice));

        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Discount percent must be between 0 and 100.", nameof(discountPercent));

        if (eveningWeekendSupplement < 0)
            throw new ArgumentException("Evening/weekend supplement cannot be negative.", nameof(eveningWeekendSupplement));

        return new PriceCalculation
        {
            BasePrice = basePrice,
            DiscountPercent = discountPercent,
            EveningWeekendSupplement = eveningWeekendSupplement,
            DiscountType = discountType
        };
    }
}