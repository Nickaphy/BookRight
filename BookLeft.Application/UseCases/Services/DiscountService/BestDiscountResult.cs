using BookRight.Domain.Enums;

namespace BookRight.Application.UseCases.Services.DiscountService;

// Stores the best discount discovered
// across all evaluated discount strategies.
//
// Thread-safe because multiple strategies
// may run in parallel.
public sealed class BestDiscountResult
{
    private readonly Lock _gate = new();

    private decimal _bestDiscount;

    private DiscountType _winningDiscountType =
        DiscountType.None;

    // Highest discount amount found.
    public decimal BestDiscount
    {
        get
        {
            lock (_gate)
            {
                return _bestDiscount;
            }
        }
    }

    // Winning discount type.
    public DiscountType WinningDiscountType
    {
        get
        {
            lock (_gate)
            {
                return _winningDiscountType;
            }
        }
    }

    // Registers a discount offer from a strategy.
    public void OfferDiscount(
        DiscountType discountType,
        decimal discount)
    {
        lock (_gate)
        {
            if (discount > _bestDiscount)
            {
                _bestDiscount = discount;

                _winningDiscountType = discountType;
            }
        }
    }
}