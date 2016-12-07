﻿// ReSharper disable InconsistentNaming
using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class ApprenticeshipPriceEpisodePeriodisedValuesEntity
    {
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public DateTime EpisodeStartDate { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public string AttributeName { get; set; }
        public decimal Period_1 { get; set; }
        public decimal Period_2 { get; set; }
        public decimal Period_3 { get; set; }
        public decimal Period_4 { get; set; }
        public decimal Period_5 { get; set; }
        public decimal Period_6 { get; set; }
        public decimal Period_7 { get; set; }
        public decimal Period_8 { get; set; }
        public decimal Period_9 { get; set; }
        public decimal Period_10 { get; set; }
        public decimal Period_11 { get; set; }
        public decimal Period_12 { get; set; }

    }
}