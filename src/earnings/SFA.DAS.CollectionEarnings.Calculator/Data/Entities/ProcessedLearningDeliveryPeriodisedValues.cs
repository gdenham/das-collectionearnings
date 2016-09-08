// ReSharper disable InconsistentNaming

using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Data.Entities
{
    public class ProcessedLearningDeliveryPeriodisedValues
    {
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
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

        public void SetPeriodValue(int period, decimal value)
        {
            switch (period)
            {
                case 1:
                    Period_1 = value;
                    break;
                case 2:
                    Period_2 = value;
                    break;
                case 3:
                    Period_3 = value;
                    break;
                case 4:
                    Period_4 = value;
                    break;
                case 5:
                    Period_5 = value;
                    break;
                case 6:
                    Period_6 = value;
                    break;
                case 7:
                    Period_7 = value;
                    break;
                case 8:
                    Period_8 = value;
                    break;
                case 9:
                    Period_9 = value;
                    break;
                case 10:
                    Period_10 = value;
                    break;
                case 11:
                    Period_11 = value;
                    break;
                case 12:
                    Period_12 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected period value: {period}");
            }
        }

        public decimal GetPeriodValue(int period)
        {
            switch (period)
            {
                case 1:
                    return Period_1;
                case 2:
                    return Period_2;
                case 3:
                    return Period_3;
                case 4:
                    return Period_4;
                case 5:
                    return Period_5;
                case 6:
                    return Period_6;
                case 7:
                    return Period_7;
                case 8:
                    return Period_8;
                case 9:
                    return Period_9;
                case 10:
                    return Period_10;
                case 11:
                    return Period_11;
                case 12:
                    return Period_12;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected period value: {period}");
            }
        }
    }
}