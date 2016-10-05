using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.AddLearnerCommitmentsCommand;
using SFA.DAS.CollectionEarnings.DataLock.Application.Learner.GetProviderLearnersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider;
using SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery;
using SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.DataLockProcessor
{
    public class WhenProcessingValidScenario
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new Learner[] {}}
        };

        private static readonly Provider[] Providers =
        {
            new Provider
            {
                Ukprn = 10007459
            },
            new Provider
            {
                Ukprn = 10007458
            }
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
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Provider
                        {
                            Ukprn = 10007459
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderCommitmentsQueryRequest>()))
                .Returns(new GetProviderCommitmentsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new CommitmentBuilder().Build()
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderLearnersQueryRequest>()))
                .Returns(new GetProviderLearnersQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new LearnerBuilder().Build()
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
        public void ThenItShouldCallGetProviderCommitmentsQueryMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetProviderCommitmentsQueryRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<GetProviderCommitmentsQueryRequest>(it => it.Ukprn == Providers[0].Ukprn)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetProviderCommitmentsQueryRequest>(it => it.Ukprn == Providers[1].Ukprn)), Times.Once);
        }

        [Test]
        public void ThenItShouldCallGetProviderLearnersQueryMultipleTimesForMultipleProviders()
        {
            // Arrange

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetProviderLearnersQueryRequest>()), Times.Exactly(2));
            _mediator.Verify(m => m.Send(It.Is<GetProviderLearnersQueryRequest>(it => it.Ukprn == Providers[0].Ukprn)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetProviderLearnersQueryRequest>(it => it.Ukprn == Providers[1].Ukprn)), Times.Once);
        }

        [Test]
        public void ThenItShouldRunDataLockValidationQueryMultipleTimesForMultipleProviders()
        {
            // Arrange

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<RunDataLockValidationQueryRequest>()), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldCallAddValidationErrorsCommandMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AddValidationErrorsCommandRequest>()), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldCallAddLearnerCommitmentsCommandMultipleTimesForMultipleProviders()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = Providers
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AddLearnerCommitmentsCommandRequest>()), Times.Exactly(2));
        }

        [Test]
        [TestCaseSource(nameof(EmptyItems))]
        public void ThenNoDataLockValidationIsExecutedForGetProviderLearnersQueryResponseWithNoItems(Learner[] items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderLearnersQueryRequest>()))
                .Returns(new GetProviderLearnersQueryResponse
                {
                    IsValid = true,
                    Items = items
                }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.IsRegex("No learners found.")), Times.Once);
        }

        [Test]
        public void ThenOutputsCorrectLogMessagesForGetProviderLearnersQueryResponseWithItems()
        {
            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.IsRegex("Started Data Lock Processor.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Performing Data Lock Validation for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Reading commitments for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Reading learners for provider with ukprn")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started Data Lock Validation.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished Data Lock Validation.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started writing Data Lock Validation Errors.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished writing Data Lock Validation Errors.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Started writing matching Learners and Commitments.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished writing matching Learners and Commitments.")), Times.Once);
            _logger.Verify(l => l.Info(It.IsRegex("Finished Data Lock Processor.")), Times.Once);

            _logger.Verify(l => l.Info(It.IsRegex("No learners found.")), Times.Never);
        }
    }
}