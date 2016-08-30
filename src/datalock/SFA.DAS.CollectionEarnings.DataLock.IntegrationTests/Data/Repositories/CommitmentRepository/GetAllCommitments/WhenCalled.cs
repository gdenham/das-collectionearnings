﻿using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Data;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Data.Repositories.CommitmentRepository.GetAllCommitments
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = ConnectionStringFactory.GetTransientConnectionString();

        private readonly Commitment[] _commitments =
        {
            new Commitment
            {
                CommitmentId = "C-001",
                Uln = 1000000019,
                Ukprn = 10007459,
                AccountId = "A-001",
                StartDate = new DateTime(2016, 9, 1),
                EndDate = new DateTime(2018, 12, 31),
                AgreedCost = 15000.00m,
                StandardCode = null,
                ProgrammeType = 20,
                FrameworkCode = 550,
                PathwayCode = 6
            },
            new Commitment
            {
                CommitmentId = "C-002",
                Uln = 1000000027,
                Ukprn = 10007459,
                AccountId = "A-002",
                StartDate = new DateTime(2016, 9, 15),
                EndDate = new DateTime(2018, 12, 15),
                AgreedCost = 30000.00m,
                StandardCode = null,
                ProgrammeType = 20,
                FrameworkCode = 550,
                PathwayCode = 6
            }
        };

        private ICommitmentRepository _commitmentRepository;

        [SetUp]
        public void Arrange()
        {
            Database.Clean(_transientConnectionString);

            _commitmentRepository = new DataLock.Data.Repositories.CommitmentRepository(_transientConnectionString);
        }

        [Test]
        public void ThenNoCommitmentsReturnedNoEntriesInTheDatabase()
        {
            // Act
            var commitments = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(0, commitments.Count());
        }

        [Test]
        public void ThenCommitmentReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            Database.AddCommitment(_transientConnectionString, _commitments[0]);

            // Act
            var response = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
        }

        [Test]
        public void ThenCommitmentsReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            Database.AddCommitment(_transientConnectionString, _commitments[0]);
            Database.AddCommitment(_transientConnectionString, _commitments[1]);

            // Act
            var response = _commitmentRepository.GetAllCommitments();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count());
        }
    }
}