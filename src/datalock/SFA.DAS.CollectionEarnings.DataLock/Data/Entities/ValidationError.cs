using Dapper.Contrib.Extensions;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Entities
{
    [Table("DataLock.ValidationError")]
    public class ValidationError
    {
        public string LearnRefNumber { get; set; }
        public long? AimSeqNumber { get; set; }
        public string RuleId { get; set; }

        public static string SelectAll = "SELECT * FROM [DataLock].[ValidationError]";
    }
}
