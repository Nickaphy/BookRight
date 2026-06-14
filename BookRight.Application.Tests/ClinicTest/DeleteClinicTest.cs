using BookRight.Application.Repositories;
using BookRight.Application.UseCases.ClinicUseCases;
using BookRight.Domain.Entities.Clinics;
using BookRight.Domain.ValueObjects;
using BookRight.Facade.Dtos.CommandDto.ClinicCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.ClinicTest
{
    public class DeleteClinicTest
    {
        private readonly Mock<IClinicRepository> _mockRepository;
        private readonly DeleteClinicHandler _handler;
        public DeleteClinicTest()
        {
            _mockRepository = new Mock<IClinicRepository>();
            _handler = new DeleteClinicHandler(_mockRepository.Object);
        }
        [Fact]
        public async Task HandleAsync_ValidCommand_DeletesClinicAndCallsRepository()
        {
            // Arrange
            var clinicId = Guid.NewGuid();
            var existingClinic = Clinic.Create(
                "Sunshine Clinic",
                5,
                "Vestergade 1",
                "Vejle",
                "7100",
                new ClinicOpeningHour[]
                {
            new ClinicOpeningHour(DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(16, 0))
                }
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(clinicId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClinic);

            _mockRepository
                .Setup(r => r.DeleteAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var request = new DeleteClinicRequest(clinicId);

            // Act
            await _handler.HandleAsync(request);

            // Assert
            _mockRepository.Verify(
                r => r.DeleteAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
