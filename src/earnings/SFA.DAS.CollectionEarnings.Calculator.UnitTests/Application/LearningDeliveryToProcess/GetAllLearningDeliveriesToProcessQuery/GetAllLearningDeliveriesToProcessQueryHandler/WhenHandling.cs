using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler
{
    public class WhenHandling
    {
        private Mock<ILearningDeliveryToProcessRepository> _learningDeliveryRepository;
        private Mock<IFinancialRecordRepository> _financialRecordRepoository; 

        private GetAllLearningDeliveriesToProcessQueryRequest _request;
        private Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _learningDeliveryRepository = new Mock<ILearningDeliveryToProcessRepository>();
            _financialRecordRepoository = new Mock<IFinancialRecordRepository>();

            _request = new GetAllLearningDeliveriesToProcessQueryRequest();

            _handler = new Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery.GetAllLearningDeliveriesToProcessQueryHandler(_learningDeliveryRepository.Object, _financialRecordRepoository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _learningDeliveryRepository
                .Setup(r => r.GetAllLearningDeliveriesToProcess())
                .Returns(new[] { new Infrastructure.Data.Entities.LearningDeliveryToProcess() });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _learningDeliveryRepository
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