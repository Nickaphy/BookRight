using BookRight.Application.Repositories;
using BookRight.Application.Services;
using BookRight.Domain.Exceptions;
using BookRight.Domain.ValueObjects;
using Moq;

namespace BookRight.Application.Tests.Services;

public class BookingConflictCheckerTests
{
    [Fact]
    public async Task EnsurePractitionerAvailabilityAsync_WhenOverlapExists_ThrowsDomainException()
    {
        // Arrange
        var repository = new Mock<IBookingRepository>();

        repository
            .Setup(x => x.HasOverlappingBookingForPractitionerAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var checker = new BookingConflictChecker(
            repository.Object);

        var timeRange = CreateTimeRange();

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        checker.EnsurePractitionerAvailabilityAsync(
                Guid.NewGuid(),
                timeRange));
    }

    [Fact]
    public async Task EnsurePractitionerAvailabilityAsync_WhenNoOverlap_DoesNotThrow()
    {
        // Arrange
        var repository = new Mock<IBookingRepository>();

        repository
            .Setup(x => x.HasOverlappingBookingForPractitionerAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var checker = new BookingConflictChecker(
            repository.Object);

        var timeRange = CreateTimeRange();

        // Act
        await checker.EnsurePractitionerAvailabilityAsync(
            Guid.NewGuid(),
            timeRange);

        // Assert

        // Verify repository was called exactly once.
        repository.Verify(
            x => x.HasOverlappingBookingForPractitionerAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task EnsureClinicAvailabilityAsync_WhenOverlapExists_ThrowsDomainException()
    {
        // Arrange
        var repository = new Mock<IBookingRepository>();

        repository
            .Setup(x => x.HasOverlappingBookingForClinicAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var checker = new BookingConflictChecker(
            repository.Object);

        var timeRange = CreateTimeRange();

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        checker.EnsureClinicAvailabilityAsync(
                Guid.NewGuid(),
                timeRange));
    }

    [Fact]
    public async Task EnsureClinicAvailabilityAsync_WhenNoOverlap_DoesNotThrow()
    {
        // Arrange
        var repository = new Mock<IBookingRepository>();

        repository
            .Setup(x => x.HasOverlappingBookingForClinicAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var checker = new BookingConflictChecker(
            repository.Object);

        var timeRange = CreateTimeRange();

        // Act
        await checker.EnsureClinicAvailabilityAsync(
            Guid.NewGuid(),
            timeRange);

        // Assert
        repository.Verify(
            x => x.HasOverlappingBookingForClinicAsync(
                It.IsAny<Guid>(),
                It.IsAny<TimeRange>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static TimeRange CreateTimeRange()
    {
        return new TimeRange(
            DateTime.Today.AddHours(10),
            DateTime.Today.AddHours(11));
    }
}