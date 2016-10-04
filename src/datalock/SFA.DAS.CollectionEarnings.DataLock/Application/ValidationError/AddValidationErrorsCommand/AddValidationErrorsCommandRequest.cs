using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandRequest : IRequest<AddValidationErrorsCommandResponse>
    {
        public Infrastructure.Data.Entities.ValidationErrorEntity[] ValidationErrors { get; set; }
    }
}