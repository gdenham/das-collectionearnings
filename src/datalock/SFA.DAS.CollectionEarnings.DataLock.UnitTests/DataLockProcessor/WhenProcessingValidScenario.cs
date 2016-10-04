using System.Collections.Generic;
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
    public class WhenProcessingValidScenario
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
        [TestCaseSource(nameof(EmptyItems))]
        public void ThenNoDataLockValidationIsExecutedForGetAllDasLearnersQueryResponseWithNoItems(LearnerEntity[] items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearnersQueryRequest>()))
                .Returns(new GetAllLearnersQueryResponse
                    {
                        IsValid = true,
                        Items = items
                    }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("No learners found."))), Times.Once);
        }

        [Test]
        public void ThenDataLockValidationIsSuccessfullForGetAllDasLearnersQueryResponseWithItems()
        {
            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Reading commitments."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Reading learners."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Started Data Lock Validation."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Finished Data Lock Validation."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Started writing Data Lock Validation Errors."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Finished writing Data Lock Validation Errors."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Started writing matching Learners and Commitments."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Finished writing matching Learners and Commitments."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("No learners found."))), Times.Never);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Finished Data Lock Processor."))), Times.Once);
        }
    }
}