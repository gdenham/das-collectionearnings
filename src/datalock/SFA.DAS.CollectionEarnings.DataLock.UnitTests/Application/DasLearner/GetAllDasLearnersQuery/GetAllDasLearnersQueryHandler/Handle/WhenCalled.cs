using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DasLearner.GetAllDasLearnersQuery.GetAllDasLearnersQueryHandler.Handle
{
    public class WhenCalled
    {
        private Mock<IDasLearnerRepository> _dasLearnerRepository;

        private GetAllDasLearnersQueryRequest _request;
        private CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery.GetAllDasLearnersQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dasLearnerRepository = new Mock<IDasLearnerRepository>();

            _request = new GetAllDasLearnersQueryRequest();

            _handler = new CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery.GetAllDasLearnersQueryHandler(_dasLearnerRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _dasLearnerRepository
                .Setup(dlr => dlr.GetAllDasLearners())
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
                .Setup(dlr => dlr.GetAllDasLearners())
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