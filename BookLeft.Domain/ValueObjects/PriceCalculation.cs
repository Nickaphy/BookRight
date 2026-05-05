using BookRight.Domain.Enums;

namespace BookRight.Domain.ValueObjects;

public class PriceCalculation
{
    // The original price of the treatment before any discounts or supplements
    public Money BasePrice { get; private set; } = null!;
    
    // The amount deducted from the base price due to a discount
    public Money DiscountAmount { get; private set; } = null!;
    
    // Which type of discount was applied (None, Loyalty, Birthday or Campaign)
    public DiscountType AppliedDiscountType { get; private set; }
    
    // The extra charge added for evening or weekend bookings
    public Money Supplement { get; private set; } = null!;
    
    // The final price the customer pays: BasePrice - DiscountAmount + Supplement
    public Money FinalPrice { get; private set; } = null!;

    // Private constructor so EF Core can reconstruct this object from the database
    // Cannot be called from outside this class
    private PriceCalculation() { }

    // Static factory method — the only way to create a PriceCalculation from outside
    // Receives all the information needed to calculate the final price
    public static PriceCalculation Create(
        Money basePrice,                      // The base price of the treatment
        LoyaltyLevel loyaltyLevel,            // The customer's loyalty level
        bool isBirthdayMonth,                 // Is the booking in the customer's birth month?
        bool isEveningOrWeekend,              // Is the booking in the evening or on a weekend?
        decimal? campaignDiscountPercent)     // Active campaign discount if any, null if none
    {
        // Ensure a base price was actually provided
        ArgumentNullException.ThrowIfNull(basePrice);

        // Campaign discount must be a valid percentage if provided
        if (campaignDiscountPercent is < 0 or > 100)
            throw new ArgumentException("Campaign discount percent must be between 0 and 100.", nameof(campaignDiscountPercent));

        // Determine the discount percentage for each discount type
        // Birthday discount is 25% if booking is in the customer's birth month, otherwise 0
        decimal birthdayPercent = isBirthdayMonth ? 25m : 0m;
        
        // Loyalty discount depends on the customer's loyalty level
        decimal loyaltyPercent = loyaltyLevel switch
        {
            LoyaltyLevel.Bronze => 5m,   // 5% for Bronze
            LoyaltyLevel.Silver => 10m,  // 10% for Silver
            LoyaltyLevel.Gold   => 15m,  // 15% for Gold
            _                   => 0m    // 0% for Standard (no discount)
        };
        
        // Use campaign discount if one exists, otherwise 0
        decimal campaignPercent = campaignDiscountPercent ?? 0m;

        // Find the highest discount percentage, only the best discount is applied, they do not stack
        decimal maxPercent = Math.Max(birthdayPercent, Math.Max(loyaltyPercent, campaignPercent));

        // Determine which discount type won
        // Priority if two discounts are equal: Birthday → Loyalty → Campaign
        DiscountType appliedType;
        if (maxPercent == 0m)
            appliedType = DiscountType.None;           // No discount applies
        else if (birthdayPercent == maxPercent)
            appliedType = DiscountType.Birthday;       // Birthday gave the best discount
        else if (loyaltyPercent == maxPercent)
            appliedType = DiscountType.Loyalty;        // Loyalty gave the best discount
        else
            appliedType = DiscountType.Campaign;       // Campaign gave the best discount

        // Calculate the actual money amounts
        // Discount amount = BasePrice * winning discount percentage
        decimal discountAmount = basePrice.Amount * maxPercent / 100m;
        
        // Supplement = 15% of BasePrice for evening or weekend bookings, otherwise 0
        // Note: supplement is calculated on the original BasePrice, not the discounted price
        decimal supplement = isEveningOrWeekend ? basePrice.Amount * 0.15m : 0m;
        
        // Final price = BasePrice minus discount plus any evening/weekend supplement
        decimal finalPrice = basePrice.Amount - discountAmount + supplement;

        // Build and return the completed PriceCalculation object
        return new PriceCalculation
        {
            BasePrice           = basePrice,
            DiscountAmount      = new Money(discountAmount),
            AppliedDiscountType = appliedType,
            Supplement          = new Money(supplement),
            FinalPrice          = new Money(finalPrice)
        };
    }
}