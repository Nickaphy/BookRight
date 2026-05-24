using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Domain.Enums;

namespace BookRight.Application.Tests.Pricing;

// Simple test builder for creating BestDiscountResult objects.
public class BestDiscountResultBuilder
{
    private readonly BestDiscountResult _result =
        new();

    public BestDiscountResultBuilder WithDiscount(
        DiscountType discountType,
        decimal amount)
    {
        _result.OfferDiscount(
            discountType,
            amount);

        return this;
    }

    public BestDiscountResult Build()
    {
        return _result;
    }
}