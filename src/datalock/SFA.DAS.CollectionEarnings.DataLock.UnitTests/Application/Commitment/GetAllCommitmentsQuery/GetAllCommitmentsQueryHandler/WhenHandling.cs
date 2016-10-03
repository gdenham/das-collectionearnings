using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Commitment.GetAllCommitmentsQuery.GetAllCommitmentsQueryHandler
{
    public class WhenHandling
    {
        private Mock<ICommitmentRepository> _commitmentRepository;

        private GetAllCommitmentsQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery.GetAllCommitmentsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _commitmentRepository = new Mock<ICommitmentRepository>();

            _request = new GetAllCommitmentsQueryRequest();

            _handler = new CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery.GetAllCommitmentsQueryHandler(_commitmentRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _commitmentRepository
                .Setup(cr => cr.GetAllCommitments())
                .Returns(new[] {new CommitmentBuilder().Build()});

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
            _commitmentRepository
                .Setup(cr => cr.GetAllCommitments())
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