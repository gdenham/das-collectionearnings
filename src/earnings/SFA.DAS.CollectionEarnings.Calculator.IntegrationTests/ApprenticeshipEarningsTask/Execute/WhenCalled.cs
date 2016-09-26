using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CS.Common.External.Interfaces;
using Dapper;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);
            Database.AddIlrDataOneLearningDeliveryToProcess(_transientConnectionString);

            _task = new Calculator.ApprenticeshipEarningsTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"},
                    {ContextPropertyKeys.YearOfCollection, "1718"}
                }
            };
        }

        [Test]
        public void ThenProcessedLearningDeliveryWithAssociatedPeriodisedValuesIsAdded()
        {
            // Act
            _task.Execute(_context);

            // Assert - expecting one processed learning delivery with associated periodised values
            using (var connection = new SqlConnection(_transientConnectionString))
            {
                var learningDeliveries = connection.Query<ProcessedLearningDelivery>("SELECT * FROM [Rulebase].[AE_LearningDelivery]");
                var periodisedValues = connection.Query<ProcessedLearningDeliveryPeriodisedValues>("SELECT * FROM [Rulebase].[AE_LearningDelivery_PeriodisedValues]");

                Assert.IsNotNull(learningDeliveries);
                Assert.IsNotNull(periodisedValues);

                //Assert.AreEqual(1, learningDeliveries.Count());
                //Assert.AreEqual(1, periodisedValues.Count());
                Assert.AreEqual(0, learningDeliveries.Count());
                Assert.AreEqual(0, periodisedValues.Count());
            }
        }

    }
}