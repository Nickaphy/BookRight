using BookRight.Application.Repositories;
using BookRight.Application.UseCases.BookingCommands;
using BookRight.Application.UseCases.CustomerCommands;
using BookRight.Domain.Common;
using BookRight.Domain.Entities.Bookings;
using BookRight.Domain.Entities.Customers;
using BookRight.Domain.Enums;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;
using BookRight.Infrastructure.Persistence;
using BookRight.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
using System.Timers;
using Xunit;

namespace EndToEndTesting.BookingEnd2EndTest
{
    public class CompleteBookingEventHandlerTest
    {
        private readonly ServiceProvider _serviceProvider;

        public CompleteBookingEventHandlerTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("E2ETest_" + Guid.NewGuid()));

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IDomainEventHandler<BookingCompletedEvent>, UpdateCustomerLoyaltyLevelHandler>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task CompleteBooking_ShouldUpgradeCustomerToBronze_WhenTotalSpentReaches3000()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var customer = Customer.Create(
                "Test Kunde",
                "12345678",
                "test@test.com",
                LoyaltyLevel.None,
                new DateTime(1990, 1, 1),
                null,
                "Testvej 1",
                "Testby",
                "1234");

            await db.Customers.AddAsync(customer);

            var timeRange = new TimeRange(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1));

            var booking = Booking.Create(
                customer.Id,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                timeRange,
                new Money(3000m),
                false);

            booking.SetFinalPrice(new Money(3000m), DiscountType.None);

            await db.Bookings.AddAsync(booking);
            await db.SaveChangesAsync();



            // Act
            
            var bookingRepo = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
            var handler = new CompleteBookingCommandHandler(bookingRepo);

            await handler.ExecuteAsync(new CompleteBookingRequest(booking.Id));

            // Assert
            var updatedCustomer = await db.Customers
                .FindAsync(customer.Id);

            Assert.Equal(LoyaltyLevel.Bronze, updatedCustomer!.LoyaltyLevel);

        }
       
    }
}
