﻿using SFA.DAS.CollectionEarnings.Calculator.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Data
{
    public interface ILearningDeliveryToProcessRepository
    {
        LearningDeliveryToProcess[] GetAllLearningDeliveriesToProcess();
    }
}