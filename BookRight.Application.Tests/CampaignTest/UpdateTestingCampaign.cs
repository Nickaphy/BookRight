using BookRight.Application.Repositories;
using BookRight.Application.UseCases.CampaignUseCases;
using BookRight.Domain.Entities.Campaigns;
using BookRight.Facade.Dtos.CommandDto.CampaignCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.CampaignTest
{
    public class UpdateTestingCampaign
    {
        private readonly Mock<ICampaignRepository> _mockRepository;
        private readonly UpdateCampaignHandler _handler;
        public UpdateTestingCampaign()
        {
            _mockRepository = new Mock<ICampaignRepository>();
            _handler = new UpdateCampaignHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_UpdatesCampaignAndCallsRepository()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var existingCampaign = new Campaign(
                "Old Campaign",
                new DateOnly(2026, 1, 1),
                new DateOnly(2026, 3, 1),
                10
            );

            var command = new UpdateCampaignCommand(
                campaignId,
                "Spring Campaign",
                new DateOnly(2026, 6, 1),
                new DateOnly(2026, 8, 31),
                20
            );

            Campaign? capturedCampaign = null;

            _mockRepository
                .Setup(r => r.GetByIdAsync(campaignId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCampaign);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Campaign>(), It.IsAny<CancellationToken>()))
                .Callback<Campaign, CancellationToken>((campaign, _) => capturedCampaign = campaign)
                .Returns(Task.CompletedTask);

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _mockRepository.Verify(
                r => r.UpdateAsync(It.IsAny<Campaign>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

            Assert.NotNull(capturedCampaign);
            Assert.Equal("Spring Campaign", capturedCampaign.Name);
            Assert.Equal(20, capturedCampaign.DiscountPercent);
        }
    }
}
