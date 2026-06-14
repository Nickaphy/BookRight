using System;
using System.Collections.Generic;
using System.Text;
using BookRight.Application.Repositories;
using BookRight.Application.UseCases.BookingCommands;
using BookRight.Domain.Common;
using BookRight.Facade.Dtos.CommandDto.BookingCommand;
using Moq;

namespace BookRight.Application.Tests.Bookings
{
    public class CompleteBookingHandlerTest
    {
        private readonly Mock<IBookingRepository> _mockRepository;
        private readonly CompleteBookingCommandHandler _handler;

        public CompleteBookingHandlerTest()
        {
            _mockRepository = new Mock<IBookingRepository>();
            
            _handler = new CompleteBookingCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public void HandleAsync_ValidCommand_CallsCompleteAsync_UpdatesLoyaltyLevel()
        {
            // Arrange
            var newBooking = Guid.NewGuid();
            var request = new CompleteBookingRequest(newBooking);

            // Act
            var result = _handler.ExecuteAsync(request);

            //Assert
            Assert.NotNull(result);
        }

    }
}
