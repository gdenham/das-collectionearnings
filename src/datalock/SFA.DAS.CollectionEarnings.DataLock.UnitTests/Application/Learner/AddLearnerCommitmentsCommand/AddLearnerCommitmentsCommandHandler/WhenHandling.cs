﻿using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Learner.AddLearnerCommitmentsCommand.AddLearnerCommitmentsCommandHandler
{
    public class WhenHandling
    {
        private static readonly LearnerCommitment[] LearnerCommitments = 
        {
            new LearnerCommitment
            {
                Ukprn = 10007459,
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 1,
                CommitmentId = "C-001"
            },
            new LearnerCommitment
            {
                Ukprn = 10007459,
                LearnerReferenceNumber = "Lrn002",
                AimSequenceNumber = 9,
                CommitmentId = "C-002"
            }
        };

        private Mock<ILearnerCommitmentRepository> _repository;

        private AddLearnerCommitmentsCommandRequest _request;
        private CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand.AddLearnerCommitmentsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<ILearnerCommitmentRepository>();

            _request = new AddLearnerCommitmentsCommandRequest
            {
                LearnerCommitments = LearnerCommitments
            };

            _handler = new CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand.AddLearnerCommitmentsCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidAddLearnerCommitmentsCommandResponseReturnedForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
        }

        [Test]
        public void ThenItShouldWriteTheLearnerCommitmentsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddLearnerCommitments(It.Is<LearnerCommitmentEntity[]>(l => LearnerCommitmentBatchesMatch(l, LearnerCommitments))), Times.Once);
        }

        [Test]
        public void ThenInvalidAddLearnerCommitmentsCommandResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(ver => ver.AddLearnerCommitments(It.IsAny<LearnerCommitmentEntity[]>()))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
        }

        private bool LearnerCommitmentBatchesMatch(LearnerCommitmentEntity[] entities, LearnerCommitment[] learnerCommitments)
        {
            if (entities.Length != learnerCommitments.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!LearnerCommitmentsMatch(entities[x], learnerCommitments[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool LearnerCommitmentsMatch(LearnerCommitmentEntity entity, LearnerCommitment learnerCommitment)
        {
            return entity.Ukprn == learnerCommitment.Ukprn &&
                   entity.LearnRefNumber == learnerCommitment.LearnerReferenceNumber &&
                   entity.AimSeqNumber == learnerCommitment.AimSequenceNumber &&
                   entity.CommitmentId == learnerCommitment.CommitmentId;
        }
    }
}