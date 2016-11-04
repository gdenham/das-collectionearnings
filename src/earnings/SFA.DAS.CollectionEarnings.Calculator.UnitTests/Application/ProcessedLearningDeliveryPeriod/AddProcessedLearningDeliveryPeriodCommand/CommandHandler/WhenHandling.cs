using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod;
using SFA.DAS.CollectionEarnings.Calculator.Application.ProcessedLearningDeliveryPeriod.AddProcessedLearningDeliveryPeriodCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ProcessedLearningDeliveryPeriod.AddProcessedLearningDeliveryPeriodCommand.CommandHandler
{
    public class WhenHandling
    {
        private static readonly LearningDeliveryPeriodEarning[] PeriodEarnings =
        {
            new LearningDeliveryPeriodEarning
            {
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                Period = 1,
                OnProgrammeEarning = 1000.00m,
                BalancingEarning = 0.00m,
                CompletionEarning = 0.00m
            },
            new LearningDeliveryPeriodEarning
            {
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                Period = 2,
                OnProgrammeEarning = 1000.00m,
                BalancingEarning = 2000.00m,
                CompletionEarning = 3000.00m
            },
            new LearningDeliveryPeriodEarning
            {
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                Period = 3,
                OnProgrammeEarning = 0.00m,
                BalancingEarning = 0.00m,
                CompletionEarning = 0.00m
            }
        };

        private Mock<IProcessedLearningDeliveryPeriodRepository> _repository;

        private AddProcessedLearningDeliveryPeriodCommandRequest _request;
        private AddProcessedLearningDeliveryPeriodCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IProcessedLearningDeliveryPeriodRepository>();

            _request = new AddProcessedLearningDeliveryPeriodCommandRequest
            {
                PeriodEarnings = PeriodEarnings
            };

            _handler = new AddProcessedLearningDeliveryPeriodCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenSuccessfullForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenItShouldWriteTheLearningDeliveryPeriodEarningsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddProcessedLearningDeliveryPeriod(It.Is<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriod[]>(l => PeriodEarningsBatchesMatch(l, PeriodEarnings))), Times.Once);
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddProcessedLearningDeliveryPeriod(It.IsAny<Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriod[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool PeriodEarningsBatchesMatch(Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriod[] entities, LearningDeliveryPeriodEarning[] periodEarnings)
        {
            if (entities.Length != periodEarnings.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PeriodEarningsMatch(entities[x], periodEarnings[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PeriodEarningsMatch(Infrastructure.Data.Entities.ProcessedLearningDeliveryPeriod entity, LearningDeliveryPeriodEarning periodEarning)
        {
            return entity.LearnRefNumber == periodEarning.LearnerReferenceNumber &&
                   entity.AimSeqNumber == periodEarning.AimSequenceNumber &&
                   entity.Period == periodEarning.Period &&
                   entity.ProgrammeAimOnProgPayment == periodEarning.OnProgrammeEarning &&
                   entity.ProgrammeAimCompletionPayment == periodEarning.CompletionEarning &&
                   entity.ProgrammeAimBalPayment == periodEarning.BalancingEarning;
        }
    }
}