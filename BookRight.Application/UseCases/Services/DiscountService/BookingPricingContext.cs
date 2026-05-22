using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;

namespace BookRight.Application.UseCases.Services.DiscountService;

// Carries all pricing-related data needed by discount strategies.
// Keeps pricing policies separated from aggregates and repositories.

/* We separated pricing policy evaluation
from domain persistence and aggregate logic
by introducing an application pricing context.*/

public sealed class BookingPricingContext
{
    // The booking currently being priced.
    public Booking Booking { get; init; } = null!;

    // The customer connected to the booking.
    public Customer Customer { get; init; } = null!;

    // True when the booking occurs during the customer's birth month.
    public bool IsBirthdayMonth { get; init; }

    // Prevents birthday discount from being used multiple times per year.
    public bool HasUsedBirthdayDiscountThisYear { get; init; }

    // Used for evening/weekend supplements.
    public bool IsEveningOrWeekend { get; init; }

    // Optional active campaign discount.
    public decimal? CampaignDiscountPercent { get; init; }
}