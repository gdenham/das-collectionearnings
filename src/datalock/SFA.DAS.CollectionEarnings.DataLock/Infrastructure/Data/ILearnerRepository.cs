﻿using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data
{
    public interface ILearnerRepository
    {
        LearnerEntity[] GetAllLearners();
    }
}