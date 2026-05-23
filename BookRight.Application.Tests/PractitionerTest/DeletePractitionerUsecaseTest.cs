using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Enums;
using BookRight.Facade.Dtos.PractitionerCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.PractitionerTest
{
    public class DeletePractitionerUsecaseTest
    {
        private readonly Mock<IPractitionerRepository> _mockRepository;
        private readonly DeletePractitionerHandler _handler;

        public DeletePractitionerUsecaseTest()
        {
            _mockRepository = new Mock<IPractitionerRepository>();
            _handler = new DeletePractitionerHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_DeletesPractitioner()
        {
            // Arrange
            var practitionerId = Guid.NewGuid();

            var existingPractitioner = new Practitioner(
                "Lene",
                "Hansen@hotmail.com",
                "+4576789809",
                "AUTH007",
                AuthorizationType.Physiotherapist
            );

            var command = new DeletePractitionerCommand(practitionerId);

            _mockRepository
                .Setup(r => r.GetByIdAsync(practitionerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPractitioner);

            _mockRepository
                .Setup(r => r.DeleteAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockRepository
                .Setup(r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(
                r => r.DeleteAsync(existingPractitioner, It.IsAny<CancellationToken>()),
                Times.Once
            );

            _mockRepository.Verify(
                r => r.SaveAsync(existingPractitioner, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleAsync_PractitionerNotFound_ThrowsUseCaseException()
        {
            // Arrange
            var practitionerId = Guid.NewGuid();

            var command = new DeletePractitionerCommand(practitionerId);

            _mockRepository
                .Setup(r => r.GetByIdAsync(practitionerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Practitioner?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UseCaseException>(
                () => _handler.HandleAsync(command)
            );

            _mockRepository.Verify(
                r => r.DeleteAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Never
            );

            _mockRepository.Verify(
                r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

    }
}
