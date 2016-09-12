using System;
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
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor.Process
{
    public class WhenCalledThenExpectingException
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
        public void ForGetAllCommitmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllCommitmentsQueryRequest>()))
                .Returns(new GetAllCommitmentsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ErrorReadingCommitments));
        }

        [Test]
        public void ForGetAllDasLearnersQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllDasLearnersQueryRequest>()))
                .Returns(new GetAllDasLearnersQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ErrorReadingDasLearners));
        }

        [Test]
        public void ForGetDataLockFailuresQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetDataLockFailuresQueryRequest>()))
                .Returns(new GetDataLockFailuresQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ErrorPerformingDataLock));
        }

        [Test]
        public void ForAddValidationErrorsCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(new AddValidationErrorsCommandResponse
                {
                    IsValid = false,
                    Exception = new Exception("Error.")
                }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(DataLockExceptionMessages.ErrorWritingDataLockValidationErrors));
        }
    }
}