using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Data.Repositories
{
    public interface IValidationErrorRepository
    {
        void AddValidationError(ValidationError validationError);

        void AddValidationErrors(List<ValidationError> validationErrors);
    }
}