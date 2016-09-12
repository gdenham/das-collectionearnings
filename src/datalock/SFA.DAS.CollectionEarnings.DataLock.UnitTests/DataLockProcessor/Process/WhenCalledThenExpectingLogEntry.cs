using System.Collections.Generic;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor.Process
{
    public class WhenCalledThenExpectingLogEntry
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new DasLearner[] {}}
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
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllDasLearnersQueryRequest>()))
                .Returns(new GetAllDasLearnersQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                            {
                                new DasLearnerBuilder().Build()
                            }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<GetDataLockFailuresQueryRequest>()))
                .Returns(new GetDataLockFailuresQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                            {
                                new ValidationErrorBuilder().Build()
                            }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(new AddValidationErrorsCommandResponse
                    {
                        IsValid = true
                    }
                );
        }

        [Test]
        [TestCaseSource(nameof(EmptyItems))]
        public void ForGetAllDasLearnersQueryNoItems(IEnumerable<DasLearner> items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllDasLearnersQueryRequest>()))
                .Returns(new GetAllDasLearnersQueryResponse
                    {
                        IsValid = true,
                        Items = items
                    }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("No DAS learners found."))), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(EmptyItems))]
        public void ForFinishingProcessing(IEnumerable<DasLearner> items)
        {
            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Reading commitments."))), Times.Once);
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Reading DAS learners."))), Times.Once);
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Started Data Lock Validation."))), Times.Once);
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Finished Data Lock Validation."))), Times.Once);
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Started writing Data Lock Validation Errors."))), Times.Once);
            _logger.Verify(l => l.Debug(It.Is<string>(p => p.Equals("Finished writing Data Lock Validation Errors."))), Times.Once);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("No DAS learners found."))), Times.Never);
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Finished Data Lock Processor."))), Times.Once);
        }
    }
}