using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler
{
    public class WhenHandling
    {
        private readonly long _ukprn = 10007459;

        private Mock<ICommitmentRepository> _commitmentRepository;

        private GetProviderCommitmentsQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _commitmentRepository = new Mock<ICommitmentRepository>();

            _request = new GetProviderCommitmentsQueryRequest
            {
                Ukprn = _ukprn
            };

            _handler = new CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler(_commitmentRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _commitmentRepository
                .Setup(cr => cr.GetProviderCommitments(_ukprn))
                .Returns(new[] {new CommitmentEntityBuilder().Build()});

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
            _commitmentRepository
                .Setup(cr => cr.GetProviderCommitments(_ukprn))
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