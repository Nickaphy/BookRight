using BookRight.Domain.Enums;

namespace BookRight.Domain.ValueObjects
{
    public class PriceCalculation
    {
        public decimal BasePrice { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public decimal DiscountPercent { get; private set; }
        public decimal EveningWeekendSupplement { get; private set; } 
        public decimal DiscountPrice => BasePrice - (BasePrice * DiscountPercent / 100);

    }
}