// Unit tests for Domain logic
// Tests pure business rules without database
// No EF Core, no repositories

// Focus:
// - booking overlap rules
// - time validation
// - clinic capacity rules
// - practitioner assignment rules
// - discount rules


// BookingTests.cs
// ? tester overlap, regler, validering

using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Enums;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;

namespace BookRight.Domain.Tests.Bookings;

public class BookingTests
{
    [Fact]
    public void Create_WhenValidData_SetsCreatedStatus()
    {
        // Arrange + Act
        var booking = CreateValidBooking();

        // Assert
        Assert.Equal(
            BookingStatus.Created,
            booking.Status);
    }

    [Fact]
    public void Cancel_WhenBookingIsCreated_SetsCancelledStatus()
    {
        // Arrange
        var booking = CreateValidBooking();

        // Act
        booking.Cancel();

        // Assert
        Assert.Equal(
            BookingStatus.Cancelled,
            booking.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsDomainException()
    {
        // Arrange
        var booking = CreateValidBooking();

        booking.Cancel();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            booking.Cancel());
    }

    [Fact]
    public void Complete_WhenBookingIsCreated_SetsCompletedStatus()
    {
        // Arrange
        var booking = CreateValidBooking();

        booking.SetFinalPrice(
            new Money(850m),
            DiscountType.Gold);

        // Act
        booking.Complete();

        // Assert
        Assert.Equal(
            BookingStatus.Completed,
            booking.Status);
    }

    [Fact]
    public void MarkNoShow_WhenBookingIsCreated_SetsNoShowStatus()
    {
        // Arrange
        var booking = CreateValidBooking();

        // Act
        booking.MarkNoShow();

        // Assert
        Assert.Equal(
            BookingStatus.NoShow,
            booking.Status);
    }

    [Fact]
    public void SetFinalPrice_WhenPriceHigherThanBasePrice_ThrowsDomainException()
    {
        // Arrange
        var booking = CreateValidBooking();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            booking.SetFinalPrice(
                new Money(1500m),
                DiscountType.None));
    }

    private static Booking CreateValidBooking()
    {
        // Shared helper creates valid aggregate.
        return Booking.Create(
            customerId: Guid.NewGuid(),
            practitionerId: Guid.NewGuid(),
            clinicId: Guid.NewGuid(),
            treatmentTypeId: Guid.NewGuid(),
            timeRange: new TimeRange(
                DateTime.Today.AddHours(10),
                DateTime.Today.AddHours(11)),
            basePrice: new Money(1000m));
    }
}