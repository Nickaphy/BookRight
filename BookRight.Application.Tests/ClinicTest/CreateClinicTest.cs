using BookRight.Application.Repositories;
using BookRight.Application.UseCases.ClinicUseCases;
using BookRight.Domain.Entities.Clinics;
using BookRight.Facade.Dtos.ClinicCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.ClinicTest
{
    public class CreateClinicTest
    {
        private readonly Mock<IClinicRepository> _mockRepository;
        private readonly CreateClinicHandler _handler;
        public CreateClinicTest()
        {
            _mockRepository = new Mock<IClinicRepository>();
            _handler = new CreateClinicHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_CreatesClinicAndCallsRepository()
        {
            // Arrange
            var request = new CreateClinicRequest(
                "Sunshine Clinic",
                "Vestergade 1",
                "Vejle",
                "7100",
                5,
                new CreateOpeningHourRequest[]
                {
                   new CreateOpeningHourRequest(DayOfWeek.Monday,    new TimeOnly(8, 0), new TimeOnly(16, 0)),
                   new CreateOpeningHourRequest(DayOfWeek.Tuesday,   new TimeOnly(8, 0), new TimeOnly(16, 0)),
                   new CreateOpeningHourRequest(DayOfWeek.Wednesday, new TimeOnly(8, 0), new TimeOnly(16, 0)),
                   new CreateOpeningHourRequest(DayOfWeek.Thursday,  new TimeOnly(8, 0), new TimeOnly(16, 0)),
                   new CreateOpeningHourRequest(DayOfWeek.Friday,    new TimeOnly(8, 0), new TimeOnly(14, 0))
                }
            );


            // Arrange
            Clinic? capturedClinic = null;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()))
                .Callback<Clinic, CancellationToken>((clinic, cancelationToken) => capturedClinic = clinic)
                .Returns(Task.CompletedTask);

            _mockRepository
                .Setup(r => r.SaveAsync(It.IsAny<Clinic>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(request);

            // Assert
            Assert.NotNull(capturedClinic);
            Assert.Equal("Sunshine Clinic", capturedClinic.Name);
            Assert.Equal("Vestergade 1", capturedClinic.Street);
            Assert.Equal("Vejle", capturedClinic.City);
            Assert.Equal("7100", capturedClinic.Zipcode);
            Assert.Equal(5, capturedClinic.AmountTreatmentRooms);
            Assert.Equal(5, capturedClinic.OpeningHours.Count);
        }



    }
}
