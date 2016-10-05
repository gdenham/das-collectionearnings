using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Learner.GetProviderLearnersQuery.GetProviderLearnersQueryHandler
{
    public class WhenHandling
    {
        private readonly long _ukprn = 10007459;

        private Mock<ILearnerRepository> _dasLearnerRepository;

        private GetProviderLearnersQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery.GetProviderLearnersQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dasLearnerRepository = new Mock<ILearnerRepository>();

            _request = new GetProviderLearnersQueryRequest
            {
                Ukprn = _ukprn
            };

            _handler = new CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery.GetProviderLearnersQueryHandler(_dasLearnerRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _dasLearnerRepository
                .Setup(dlr => dlr.GetProviderLearners(_ukprn))
                .Returns(new[] {new LearnerEntityBuilder().Build()});

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _dasLearnerRepository
                .Setup(dlr => dlr.GetProviderLearners(_ukprn))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
            Assert.IsNull(response.Items);
        }
    }
}