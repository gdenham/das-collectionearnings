using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandRequest : IRequest<AddValidationErrorsCommandResponse>
    {
        public IEnumerable<Data.Entities.ValidationError> ValidationErrors { get; set; }
    }
}