using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler
{
    public class WhenHandling
    {
        // TODO: check that the domain object has the same values as the entity sent to the repository
        private Mock<IApprenticeshipPriceEpisodeRepository> _repository;

        private ProcessAddPriceEpisodesCommandRequest _request;
        private Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IApprenticeshipPriceEpisodeRepository>();

            _request = new ProcessAddPriceEpisodesCommandRequest
            {
                PriceEpisodes = new []
                {
                    new ApprenticeshipPriceEpisodeBuilder().Build(),
                    new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(2).Build(),
                    new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(3).Build(),
                    new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(4).Build(),
                    new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(5).Build()
                }
            };

            _handler = new Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddApprenticeshipPriceEpisodes(It.IsAny<Infrastructure.Data.Entities.ApprenticeshipPriceEpisodeEntity[]>()));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddApprenticeshipPriceEpisodes(It.IsAny<Infrastructure.Data.Entities.ApprenticeshipPriceEpisodeEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }
    }
}