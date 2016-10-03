using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data
{
    public interface IValidationErrorRepository
    {
        void AddValidationError(ValidationErrorEntity validationError);

        void AddValidationErrors(IEnumerable<ValidationErrorEntity> validationErrors);
    }
}