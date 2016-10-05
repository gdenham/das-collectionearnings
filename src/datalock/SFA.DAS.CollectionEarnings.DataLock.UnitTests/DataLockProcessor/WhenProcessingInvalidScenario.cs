using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetAllLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor
{
    public class WhenProcessingInvalidScenario
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new LearnerEntity[] {}}
        };

        private DataLock.DataLockProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new DataLock.DataLockProcessor(_logger.Object, _mediator.Object);

            MediatorSetup();
        }

        private void MediatorSetup()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllCommitmentsQueryRequest>()))
                .Returns(new GetAllCommitmentsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new CommitmentBuilder().Build()
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearnersQueryRequest>()))
                .Returns(new GetAllLearnersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new LearnerEntityBuilder().Build()
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()))
                .Returns(new RunDataLockValidationQueryResponse
                {
                    IsValid = true,
                    ValidationErrors = new[]
                    {
                        new ValidationErrorBuilder().Build()
                    },
                    LearnerCommitments = new[]
                    {
                        new LearnerCommitment
                        {
                            Ukprn = 10007459,
                            LearnerReferenceNumber = "Lrn001",
                            AimSequenceNumber = 1,
                            CommitmentId = "C-001"
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddLearnerCommitmentsCommandRequest>()))
                .Returns(Unit.Value);
        }

        [Test]
        public void ThenExpectingExceptionForGetAllCommitmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllCommitmentsQueryRequest>()))
                .Returns(new GetAllCommitmentsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockProcessorException.ErrorReadingCommitmentsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetAllDasLearnersQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearnersQueryRequest>()))
                .Returns(new GetAllLearnersQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockProcessorException.ErrorReadingLearnersMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetDataLockFailuresQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()))
                .Returns(new RunDataLockValidationQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                });

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockProcessorException.ErrorPerformingDataLockMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddValidationErrorsCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockProcessorException.ErrorWritingDataLockValidationErrorsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddLearnerCommitmentsCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddLearnerCommitmentsCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockProcessorException.ErrorWritingMatchingLearnersAndCommitmentsMessage));
        }
    }
}