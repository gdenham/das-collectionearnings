using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetAllCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasLearner.GetAllDasLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.GetDataLockFailuresQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.ExternalContext;
using SFA.DAS.CollectionEarnings.DataLock.Context;
using SFA.DAS.CollectionEarnings.DataLock.DependencyResolution;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockTask.Execute
{
    public class WhenCalledWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };

            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();
            MediatorSetup();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver
                .Setup(dr => dr.GetInstance<ILogger>())
                .Returns(_logger.Object);
            _dependencyResolver
                .Setup(dr => dr.GetInstance<IMediator>())
                .Returns(_mediator.Object);

            _task = new DataLock.DataLockTask(_dependencyResolver.Object);
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
        public void ThenExpectingExceptionForGetAllCommitmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllCommitmentsQueryRequest>()))
                .Returns(new GetAllCommitmentsQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception("Error while reading commitments.")
                    }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains("Error while reading commitments."));
        }

        [Test]
        public void ThenExpectingExceptionForGetAllDasLearnersQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllDasLearnersQueryRequest>()))
                .Returns(new GetAllDasLearnersQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception("Error while reading DAS specific learners.")
                    }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains("Error while reading DAS specific learners."));
        }

        [Test]
        public void ThenExpectingExceptionForGetDataLockFailuresQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetDataLockFailuresQueryRequest>()))
                .Returns(new GetDataLockFailuresQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception("Error while performing data lock.")
                    }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains("Error while performing data lock."));
        }

        [Test]
        public void ThenExpectingExceptionForAddValidationErrorsCommandRequestFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()))
                .Returns(new AddValidationErrorsCommandResponse
                    {
                        IsValid = false,
                        Exception = new Exception("Error while writing data lock validation errors.")
                    }
                );

            // Assert
            var ex = Assert.Throws<DataLockProcessorException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains("Error while writing data lock validation errors."));
        }

        [Test]
        public void ThenTaskIsExecuted()
        {
            // Act
            _task.Execute(_context);

            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}