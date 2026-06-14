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
    public class UpdateClinicTest
    {
        private readonly Mock<IClinicRepository> _mockRepository;
        private readonly UpdateClinicHandler _handler;
        public UpdateClinicTest()
        {
            _mockRepository = new Mock<IClinicRepository>();
            _handler = new UpdateClinicHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_UpdatesClinicAndCallsRepository()
        {
            // Arrange
            var clinicId = Guid.NewGuid();
            var existingClinic = Clinic.Create(
                "Old Clinic",
                3,
                "Gammel Vej 1",
                "Kolding",
                "6000",
                new ClinicOpeningHour[]
                {
            new ClinicOpeningHour(DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(16, 0))
                }
            );

            var command = new UpdateClinicRequest(
                clinicId,
                "Sunshine Clinic",
                "Vestergade 1",
                "Vejle",
                "7100",
                5,
                new CreateOpeningHourRequest[]
                {
            new CreateOpeningHourRequest(DayOfWeek.Monday,    new TimeOnly(8, 0), new TimeOnly(16, 0)),
            new CreateOpeningHourRequest(DayOfWeek.Friday,    new TimeOnly(8, 0), new TimeOnly(14, 0))
                }
            );

            Clinic? capturedClinic = null;

            _mockRepository
                .Setup(r => r.GetByIdAsync(clinicId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClinic);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()))
                .Callback<Clinic, CancellationToken>((clinic, _) => capturedClinic = clinic)
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(
                r => r.UpdateAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            Assert.NotNull(capturedClinic);
            Assert.Equal("Sunshine Clinic", capturedClinic.Name);
            Assert.Equal("Vestergade 1", capturedClinic.Street);
            Assert.Equal("Vejle", capturedClinic.City);
            Assert.Equal("7100", capturedClinic.Zipcode);
            Assert.Equal(5, capturedClinic.AmountTreatmentRooms);
        }
    }
}
