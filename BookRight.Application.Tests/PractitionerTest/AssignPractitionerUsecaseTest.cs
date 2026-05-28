using BookRight.Application.Repositories;
using BookRight.Application.UseCaseExceptions;
using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Domain.Entities.Practitioners;
using BookRight.Domain.Enums;
using BookRight.Domain.Exceptions;
using BookRight.Facade.Dtos.PractitionerCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.PractitionerTest
{
    public class AssignPractitionerUsecaseTest
    {
        private readonly Mock<IPractitionerRepository> _mockRepository;
        private readonly Mock<IClinicRepository> _mockClinicRepository; 
        private readonly AssignPractitionerToClinicHandler _handler;

        public AssignPractitionerUsecaseTest()
        {
            _mockRepository = new Mock<IPractitionerRepository>();
            _mockClinicRepository = new Mock<IClinicRepository>(); 
            _handler = new AssignPractitionerToClinicHandler(_mockRepository.Object, _mockClinicRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_AssignsPractitionerToClinicAndCallsRepository()
        {
            // Arrange
            var practitionerId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();
            var date = new DateTime(2026, 6, 1);

            var existingPractitioner = new Practitioner(
                "Lene",
                "Hansen@hotmail.com",
                "+4576789809",
                "AUTH007",
                AuthorizationType.Physiotherapist
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(practitionerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPractitioner);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockRepository
                .Setup(r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new AssignPractitionerToClinicCommand(practitionerId, clinicId, date);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(
                r => r.UpdateAsync(existingPractitioner, It.IsAny<CancellationToken>()),
                Times.Once
            );

            _mockRepository.Verify(
                r => r.SaveAsync(existingPractitioner, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Fact]
        public async Task HandleAsync_InvalidDateInPast_ThrowsExceptionAndDoesNotAssignPractitioner()
        {
            // Arrange
            var practitionerId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();
            var pastDate = new DateTime(2020, 1, 1); // Dato in past

            var existingPractitioner = new Practitioner(
                "Lene",
                "Hansen@hotmail.com",
                "+4576789809",
                "AUTH007",
                AuthorizationType.Physiotherapist
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(practitionerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPractitioner);

            var command = new AssignPractitionerToClinicCommand(practitionerId, clinicId, pastDate);

            // Act
            var act = async () => await _handler.HandleAsync(command);

            // Assert
            await Assert.ThrowsAsync<DomainException>(() => _handler.HandleAsync(command)
            );

            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Never
            );

            _mockRepository.Verify(r => r.SaveAsync(It.IsAny<Practitioner>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }
    }
}
