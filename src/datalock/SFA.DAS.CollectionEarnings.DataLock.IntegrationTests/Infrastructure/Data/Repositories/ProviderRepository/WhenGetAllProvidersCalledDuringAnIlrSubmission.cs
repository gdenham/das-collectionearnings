﻿using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.ProviderRepository
{
    public class WhenGetAllProvidersCalledDuringAnIlrSubmission
    {
        private IProviderRepository _providerRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _providerRepository = new DataLock.Infrastructure.Data.Repositories.ProviderRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenNoProvidersReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var providers = _providerRepository.GetAllProviders();

            // Assert
            Assert.IsNotNull(providers);
            Assert.AreEqual(0, providers.Length);
        }

        [Test]
        public void ThenDasLearnersReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddValidProvider(12345678);
            TestDataHelper.AddValidProvider(87654321);

            // Act
            var providers = _providerRepository.GetAllProviders();

            // Assert
            Assert.IsNotNull(providers);
            Assert.AreEqual(2, providers.Length);
            Assert.AreEqual(1, providers.Count(p => p.Ukprn == 12345678));
            Assert.AreEqual(1, providers.Count(p => p.Ukprn == 87654321));
        }
    }
}