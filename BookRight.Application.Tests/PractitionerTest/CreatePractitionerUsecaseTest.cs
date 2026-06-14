using BookRight.Application.Repositories;
using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Enums;
using BookRight.Facade.Dtos.CommandDto.PractitionerCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.PractitionerTest
{
    public class CreatePractitionerUsecaseTest
    {
        private readonly Mock<IPractitionerRepository> _mockRepository;
        private readonly CreatePractitionerHandler _handler;

        public CreatePractitionerUsecaseTest()
        {

            _mockRepository = new Mock<IPractitionerRepository>();
            _handler = new CreatePractitionerHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_CreatesPractitionerAndCallsRepository()
        {
            // Arrange
            var command = new CreatePractitionerCommand(
                "Lene",
                "Hansen@hotmail.com",
                "+4576789809",
                "AUTH007",
                "Physiotherapist"
            );

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockRepository
                .Setup(r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(
                r => r.AddAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            _mockRepository.Verify(
                r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_CreatesPractitionerViaFactoryMethod()
        {
            // Arrange
            var command = new CreatePractitionerCommand(
                "Lene",
                "Hansen@hotmail.com",
                "+4576789809",
                "AUTH007",
                "Physiotherapist"
            );

            Practitioner? capturedPractitioner = null;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Callback<Practitioner, CancellationToken>((practitioner, Cancelationtoken) => capturedPractitioner = practitioner)
                .Returns(Task.CompletedTask);

            _mockRepository
                .Setup(r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            Assert.NotNull(capturedPractitioner);
            Assert.Equal("Lene", capturedPractitioner.Name);
            Assert.Equal("Hansen@hotmail.com", capturedPractitioner.Email);
            Assert.Equal("+4576789809", capturedPractitioner.PhoneNumber);
            Assert.Equal("AUTH007", capturedPractitioner.AuthorizationCode);
            Assert.Equal(AuthorizationType.Physiotherapist, capturedPractitioner.AuthorizationType);
        }

    }
}
