using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Commitment.GetAllCommitmentsQuery.GetAllCommitmentsQueryHandler.Handle
{
    public class WhenCalled
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
                .Returns(new[]
                    {
                        new Data.Entities.Commitment
                        {
                            CommitmentId = "C-001",
                            Uln = 1000000019,
                            Ukprn = 10007459,
                            AccountId = "A-001",
                            StartDate = new DateTime(2016, 9, 1),
                            EndDate = new DateTime(2018, 12, 31),
                            AgreedCost = 15000.00m,
                            StandardCode = null,
                            ProgrammeType = 20,
                            FrameworkCode = 550,
                            PathwayCode = 6
                        }
                    }
                );

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