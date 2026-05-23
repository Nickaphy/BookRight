using BookRight.Application.Repositories;
using BookRight.Application.UseCases.CampaignUseCases;
using BookRight.Domain.Entities.Campaigns;
using BookRight.Facade.Dtos.CampaignCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BookRight.Application.Tests.CampaignTest
{
    public class CreateTestingCampaign
    {
        private readonly Mock<ICampaignRepository> _mockRepository;
        private readonly CreateCampaignHandler _handler;

        // Constructorens navn skal matche klassens navn
        public CreateTestingCampaign()
        {
            _mockRepository = new Mock<ICampaignRepository>();
            _handler = new CreateCampaignHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_CallsAddAsyncOnce()
        {
            // Arrange
            var command = new CreateCampaignCommand(

                "Summer campaign",
                new DateOnly(2025, 6, 1),
                new DateOnly(2025, 8, 31),
                20
            );

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(repos => repos.AddAsync(It.IsAny<Campaign>(), It.IsAny<CancellationToken>()),
                Times.Once //verificerer at AddAsync bliver kaldt 1 gang
            );
        }
        [Fact]
        public async Task HandleAsync_ValidCommand_CreatesCampaignAndCallsRepository()
        {
            // Arrange
            var command = new CreateCampaignCommand(
                "Spring campaign",
                new DateOnly(2025, 6, 1),
                new DateOnly(2025, 8, 31),
                20
            );

            Campaign? capturedCampaign = null;  //starting from null

            _mockRepository
                .Setup(campRepository => campRepository.AddAsync(It.IsAny<Campaign>(), It.IsAny<CancellationToken>()))
                .Callback<Campaign, CancellationToken>((campaign, cancelationToken) => capturedCampaign = campaign)
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert 

            Assert.NotNull(capturedCampaign);                       //checks that the captured campaign is made.
            Assert.Equal("Spring campaign", capturedCampaign.Name);
            Assert.Equal(new DateOnly(2025, 6, 1), capturedCampaign.StartDate);
            Assert.Equal(new DateOnly(2025, 8, 31), capturedCampaign.EndDate);
            Assert.Equal(20, capturedCampaign.DiscountPercent);
        }

        [Fact]
        public async Task HandleAsync_DuplicateName_ThrowsException()
        {
            // Arrange
            var command = new CreateCampaignCommand(
                "Spring campaign",
                new DateOnly(2025, 6, 1),
                new DateOnly(2025, 8, 31),
                20
            );

            _mockRepository
                .Setup(r => r.ExistsAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); //makes the method ExsistAsync to flick on true and provoke the exception

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.HandleAsync(command)
            );
        }
    }
}
