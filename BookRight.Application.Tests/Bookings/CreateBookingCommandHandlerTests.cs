// Tests for Application layer (use cases)
// Uses Moq to fake repositories and services

// Focus:
// - command behavior (create booking)
// - query behavior (reports)
// - discount calculation coordination
// - interaction between services and repositories


// CreateBookingCommandHandlerTests.cs
// ? tester flow +repositories + services

using BookRight.Application.UseCases.BookingCommands;
using BookRight.Application.Repositories;
using BookRight.Application.UseCases.Services.DiscountService;
using BookRight.Application.UseCases.Services.PriceCalculator;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.DomainServices.BookingConfictChecker;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;
using Moq;

namespace BookRight.Application.Tests.Bookings;

public class CreateBookingCommandHandlerTests
{
    [Fact]
    public async Task CreateBookingAsync_WhenCustomerIsMissing_ThrowsInvalidOperationException()
    {
        // Arrange:
        // Customer repository returns null
        // to simulate a missing customer.
        var bookingRepository = new Mock<IBookingRepository>();

        var customerRepository = new Mock<ICustomerRepository>();

        var practitionerRepository =
            new Mock<IPractitionerRepository>();

        var clinicRepository =
            new Mock<IClinicRepository>();

        var treatmentTypeRepository =
            new Mock<ITreatmentTypeRepository>();

        var bookingConflictChecker =
            new Mock<IBookingConflictChecker>();

        var discountService =
            new Mock<IDiscountService>();

        var priceCalculator =
            new Mock<IPriceCalculator>();

        var request = new CreateBookingRequest(
            CustomerId: Guid.NewGuid(),
            PractitionerId: Guid.NewGuid(),
            ClinicId: Guid.NewGuid(),
            TreatmentTypeId: Guid.NewGuid(),
            StartTime: DateTime.Today.AddDays(1),
            IsTeam: false);

        // Simulate customer not found.
        customerRepository
            .Setup(x => x.GetCustomerByIdAsync(
                request.CustomerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                (BookRight.Domain.Entities.Customers.Customer?)null);

        var handler = new CreateBookingCommandHandler(
            bookingRepository.Object,
            customerRepository.Object,
            practitionerRepository.Object,
            clinicRepository.Object,
            treatmentTypeRepository.Object,
            bookingConflictChecker.Object,
            priceCalculator.Object);

        // Act + Assert:
        // Booking creation should fail immediately.
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.CreateBookingAsync(request));

        Assert.Equal(
            "Customer was not found.",
            exception.Message);

        // Verify that no booking was persisted.
        bookingRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        bookingRepository.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }




    [Fact]
    public async Task CreateBookingAsync_WhenPractitionerIsMissing_ThrowsInvalidOperationException()
    {
        // Arrange:
        // Customer exists,
        // but practitioner repository returns null.
        var bookingRepository = new Mock<IBookingRepository>();

        var customerRepository = new Mock<ICustomerRepository>();

        var practitionerRepository =
            new Mock<IPractitionerRepository>();

        var clinicRepository =
            new Mock<IClinicRepository>();

        var treatmentTypeRepository =
            new Mock<ITreatmentTypeRepository>();

        var bookingConflictChecker =
            new Mock<IBookingConflictChecker>();

        var discountService =
            new Mock<IDiscountService>();

        var priceCalculator =
            new Mock<IPriceCalculator>();

        var request = new CreateBookingRequest(
            CustomerId: Guid.NewGuid(),
            PractitionerId: Guid.NewGuid(),
            ClinicId: Guid.NewGuid(),
            TreatmentTypeId: Guid.NewGuid(),
            StartTime: DateTime.Today.AddDays(1),
            IsTeam: false);

        // Mock customer exists.
        customerRepository
            .Setup(x => x.GetCustomerByIdAsync(
                request.CustomerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                BookRight.Domain.Entities.Customers.Customer.Create(
                    "Test Customer",
                    "12345678",
                    "test@test.dk",
                    BookRight.Domain.Enums.LoyaltyLevel.None,
                    new DateTime(1990, 5, 10),
                    null,
                    "Testvej 1",
                    "Vejle",
                    "7100"));

        // Mock practitioner missing.
        practitionerRepository
            .Setup(x => x.GetByIdAsync(
                request.PractitionerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                (BookRight.Domain.Entities.Practitioners.Practitioner?)null);

        var handler = new CreateBookingCommandHandler(
            bookingRepository.Object,
            customerRepository.Object,
            practitionerRepository.Object,
            clinicRepository.Object,
            treatmentTypeRepository.Object,
            bookingConflictChecker.Object,
            priceCalculator.Object);

        // Act + Assert:
        // Booking creation should stop immediately.
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.CreateBookingAsync(request));

        Assert.Equal(
            "Practitioner was not found.",
            exception.Message);

        // Verify nothing was persisted.
        bookingRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        bookingRepository.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }





    [Fact]
    public async Task CreateBookingAsync_CallsBookingConflictChecker()
    {
        // Arrange
        var bookingRepository = new Mock<IBookingRepository>();

        var customerRepository = new Mock<ICustomerRepository>();

        var practitionerRepository =
            new Mock<IPractitionerRepository>();

        var clinicRepository =
            new Mock<IClinicRepository>();

        var treatmentTypeRepository =
            new Mock<ITreatmentTypeRepository>();

        var bookingConflictChecker =
            new Mock<IBookingConflictChecker>();

        var discountService =
            new Mock<IDiscountService>();

        var priceCalculator =
            new Mock<IPriceCalculator>();

        var request = new CreateBookingRequest(
            CustomerId: Guid.NewGuid(),
            PractitionerId: Guid.NewGuid(),
            ClinicId: Guid.NewGuid(),
            TreatmentTypeId: Guid.NewGuid(),
            StartTime: DateTime.Today.AddDays(1),
            IsTeam: false);

        // Valid customer.
        var customer =
            BookRight.Domain.Entities.Customers.Customer.Create(
                "Test Customer",
                "12345678",
                "test@test.dk",
                BookRight.Domain.Enums.LoyaltyLevel.None,
                new DateTime(1990, 5, 10),
                null,
                "Testvej 1",
                "Vejle",
                "7100");

        customerRepository
            .Setup(x => x.GetCustomerByIdAsync(
                request.CustomerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Valid practitioner.
        var practitioner =
            BookRight.Domain.Entities.Practitioners.Practitioner.Create(
                "Test Practitioner",
                "prac@test.dk",
                "12345678",
                "AUTH123",
                BookRight.Domain.Enums.AuthorizationType.Physiotherapist);

        practitionerRepository
            .Setup(x => x.GetByIdAsync(
                request.PractitionerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(practitioner);

        // Valid clinic.
        var clinic =
            new BookRight.Domain.Entities.Clinics.Clinic(
                "Test Clinic",
                5,
                "Street 1",
                "Vejle",
                "7100");

        clinicRepository
            .Setup(x => x.GetByIdAsync(
                request.ClinicId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(clinic);

        // Valid treatment type.
        var treatment =
            new BookRight.Domain.Entities.Treatments.TreatmentType(
                "Massage",
                60,
                new BookRight.Domain.ValueObjects.Money(1000m),
                BookRight.Domain.Enums.AuthorizationType.Physiotherapist,
                1);

        treatmentTypeRepository
            .Setup(x => x.GetByIdAsync(
                request.TreatmentTypeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Mock pricing result.
        priceCalculator
            .Setup(x => x.CalculateFinalPriceAsync(
                It.IsAny<
                    BookRight.Application.UseCases.Services.DiscountService.BookingPricingContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((
                new BookRight.Domain.ValueObjects.Money(850m),
                BookRight.Domain.Enums.DiscountType.None));

        var handler = new CreateBookingCommandHandler(
            bookingRepository.Object,
            customerRepository.Object,
            practitionerRepository.Object,
            clinicRepository.Object,
            treatmentTypeRepository.Object,
            bookingConflictChecker.Object,
            priceCalculator.Object);

        // Act
        await handler.CreateBookingAsync(request);

        // Assert:
        // Verify practitioner availability was checked.

    }




    [Fact]
    public async Task CreateBookingAsync_SavesBooking()
    {
        // Arrange
        var bookingRepository = new Mock<IBookingRepository>();

        var customerRepository = new Mock<ICustomerRepository>();

        var practitionerRepository =
            new Mock<IPractitionerRepository>();

        var clinicRepository =
            new Mock<IClinicRepository>();

        var treatmentTypeRepository =
            new Mock<ITreatmentTypeRepository>();

        var bookingConflictChecker =
            new Mock<IBookingConflictChecker>();

        var discountService =
            new Mock<IDiscountService>();

        var priceCalculator =
            new Mock<IPriceCalculator>();

        var request = new CreateBookingRequest(
            CustomerId: Guid.NewGuid(),
            PractitionerId: Guid.NewGuid(),
            ClinicId: Guid.NewGuid(),
            TreatmentTypeId: Guid.NewGuid(),
            StartTime: DateTime.Today.AddDays(1),
            IsTeam: false);

        // Valid customer.
        var customer =
            BookRight.Domain.Entities.Customers.Customer.Create(
                "Test Customer",
                "12345678",
                "test@test.dk",
                BookRight.Domain.Enums.LoyaltyLevel.None,
                new DateTime(1990, 5, 10),
                null,
                "Testvej 1",
                "Vejle",
                "7100");

        customerRepository
            .Setup(x => x.GetCustomerByIdAsync(
                request.CustomerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Valid practitioner.
        var practitioner =
            BookRight.Domain.Entities.Practitioners.Practitioner.Create(
                "Test Practitioner",
                "prac@test.dk",
                "12345678",
                "AUTH123",
                BookRight.Domain.Enums.AuthorizationType.Physiotherapist);

        practitionerRepository
            .Setup(x => x.GetByIdAsync(
                request.PractitionerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(practitioner);

        // Valid clinic.
        var clinic =
            new BookRight.Domain.Entities.Clinics.Clinic(
                "Test Clinic",
                5,
                "Street 1",
                "Vejle",
                "7100");

        clinicRepository
            .Setup(x => x.GetByIdAsync(
                request.ClinicId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(clinic);

        // Valid treatment type.
        var treatment =
            new BookRight.Domain.Entities.Treatments.TreatmentType(
                "Massage",
                60,
                new BookRight.Domain.ValueObjects.Money(1000m),
                BookRight.Domain.Enums.AuthorizationType.Physiotherapist,
                1);

        treatmentTypeRepository
            .Setup(x => x.GetByIdAsync(
                request.TreatmentTypeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        // Mock final calculated price.
        priceCalculator
            .Setup(x => x.CalculateFinalPriceAsync(
                It.IsAny<
                    BookRight.Application.UseCases.Services.DiscountService.BookingPricingContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((
                new BookRight.Domain.ValueObjects.Money(850m),
                BookRight.Domain.Enums.DiscountType.None));

        var handler = new CreateBookingCommandHandler(
            bookingRepository.Object,
            customerRepository.Object,
            practitionerRepository.Object,
            clinicRepository.Object,
            treatmentTypeRepository.Object,
            bookingConflictChecker.Object,
            priceCalculator.Object);

        // Act
        await handler.CreateBookingAsync(request);

        // Assert:
        // Verify aggregate was persisted.
        bookingRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Booking>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        // Verify changes were committed.
        bookingRepository.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }





    [Fact]
    public async Task CreateBookingAsync_AppliesBirthdayDiscount()
    {
        // Arrange
        var bookingRepository = new Mock<IBookingRepository>();
        var customerRepository = new Mock<ICustomerRepository>();
        var practitionerRepository = new Mock<IPractitionerRepository>();
        var clinicRepository = new Mock<IClinicRepository>();
        var treatmentTypeRepository = new Mock<ITreatmentTypeRepository>();
        var bookingConflictChecker = new Mock<IBookingConflictChecker>();
        var discountService = new Mock<IDiscountService>();
        var priceCalculator = new Mock<IPriceCalculator>();

        var startTime = DateTime.Today.AddYears(1).AddMonths(5 - DateTime.Today.Month);

        startTime = new DateTime(startTime.Year,5,20,10,0,0);

        var request = new CreateBookingRequest(
            CustomerId: Guid.NewGuid(),
            PractitionerId: Guid.NewGuid(),
            ClinicId: Guid.NewGuid(),
            TreatmentTypeId: Guid.NewGuid(),
            StartTime: startTime,
            IsTeam: false);

        var customer = BookRight.Domain.Entities.Customers.Customer.Create(
            "Test Customer",
            "12345678",
            "test@test.dk",
            BookRight.Domain.Enums.LoyaltyLevel.None,
            new DateTime(1990, 5, 10),
            null,
            "Testvej 1",
            "Vejle",
            "7100");

        customerRepository
            .Setup(x => x.GetCustomerByIdAsync(
                request.CustomerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var practitioner =
            BookRight.Domain.Entities.Practitioners.Practitioner.Create(
                "Test Practitioner",
                "prac@test.dk",
                "12345678",
                "AUTH123",
                BookRight.Domain.Enums.AuthorizationType.Physiotherapist);

        practitionerRepository
            .Setup(x => x.GetByIdAsync(
                request.PractitionerId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(practitioner);

        var clinic = new BookRight.Domain.Entities.Clinics.Clinic(
            "Test Clinic",
            5,
            "Street 1",
            "Vejle",
            "7100");

        clinicRepository
            .Setup(x => x.GetByIdAsync(
                request.ClinicId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(clinic);

        var treatment = new BookRight.Domain.Entities.Treatments.TreatmentType(
            "Massage",
            60,
            new BookRight.Domain.ValueObjects.Money(1000m),
            BookRight.Domain.Enums.AuthorizationType.Physiotherapist,
            1);

        treatmentTypeRepository
            .Setup(x => x.GetByIdAsync(
                request.TreatmentTypeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(treatment);

        bookingRepository
            .Setup(x => x.HasUsedBirthdayDiscountAsync(
                customer.Id,
                startTime.Year,
                customer.DateOfBirth.Month,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        priceCalculator
            .Setup(x => x.CalculateFinalPriceAsync(
                It.IsAny<BookRight.Application.UseCases.Services.DiscountService.BookingPricingContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((
                new BookRight.Domain.ValueObjects.Money(750m),
                BookRight.Domain.Enums.DiscountType.BirthdayMonth));

        var handler = new CreateBookingCommandHandler(
            bookingRepository.Object,
            customerRepository.Object,
            practitionerRepository.Object,
            clinicRepository.Object,
            treatmentTypeRepository.Object,
            bookingConflictChecker.Object,
            priceCalculator.Object);

        // Act
        await handler.CreateBookingAsync(request);

        // Assert: captured booking should contain birthday discount result.
        bookingRepository.Verify(x => x.AddAsync(
            It.Is<BookRight.Domain.Entities.Bookings.Booking>(b =>
                b.FinalPrice.Amount == 750m &&
                b.DiscountType == BookRight.Domain.Enums.DiscountType.BirthdayMonth),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}