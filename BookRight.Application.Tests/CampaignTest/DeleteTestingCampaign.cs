using BookRight.Application.Repositories;
using BookRight.Application.UseCases.CampaignUseCases;
using BookRight.Domain.Entities.Campaigns;
using BookRight.Facade.Dtos.CampaignCommand;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.Tests.CampaignTest
{
    public class DeleteTestingCampaign
    {
        private readonly Mock<ICampaignRepository> _mockRepository;
        private readonly DeleteCampaignHandler _handler;
        public DeleteTestingCampaign()
        {
            _mockRepository = new Mock<ICampaignRepository>();
            _handler = new DeleteCampaignHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_DeletesCampaignAndCallsRepository()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var campaign = new Campaign(
                "Spring campaign",
                new DateOnly(2025, 6, 1),
                new DateOnly(2025, 8, 31),
                20
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(campaignId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(campaign);

            var command = new DeleteCampaignCommand(campaignId);

            // Act
            await _handler.HandleAsync(command);

            // Assert 
            _mockRepository.Verify(
                r => r.DeleteAsync(campaign, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }



    }
}
