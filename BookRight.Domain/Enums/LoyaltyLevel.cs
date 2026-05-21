//Describes the customer
namespace BookRight.Domain.Enums;
public enum LoyaltyLevel
{
    None,
    Bronze,
    Silver,
    Gold
}

// NOTE:
// Will later be affected by concurrency issues (race conditions)
