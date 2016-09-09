using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Data.Repositories;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler.Handle
{
    public class WhenCalled
    {
        private Mock<ILearningDeliveryToProcessRepository> _repository;

        private GetAllLearningDeliveriesToProcessQueryRequest _request;
        private Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<ILearningDeliveryToProcessRepository>();

            _request = new GetAllLearningDeliveriesToProcessQueryRequest();

            _handler = new Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler(_repository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Returns(new[] {new LearningDeliveryToProcessBuilder().Build()});

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
            _repository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Throws(new Exception("Error while reading learning deliveries to process."));

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