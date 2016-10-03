using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Learner.GetAllLearnersQuery.GetAllLearnersQueryHandler
{
    public class WhenHandling
    {
        private Mock<ILearnerRepository> _dasLearnerRepository;

        private GetAllLearnersQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery.GetAllLearnersQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dasLearnerRepository = new Mock<ILearnerRepository>();

            _request = new GetAllLearnersQueryRequest();

            _handler = new CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery.GetAllLearnersQueryHandler(_dasLearnerRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _dasLearnerRepository
                .Setup(dlr => dlr.GetAllLearners())
                .Returns(new[] {new DasLearnerBuilder().Build()});

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Count());
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _dasLearnerRepository
                .Setup(dlr => dlr.GetAllLearners())
                .Throws(new Exception("Exception while reading commitments."));

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