using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandRequest : IRequest<AddValidationErrorCommandResponse>
    {
        public Infrastructure.Data.Entities.ValidationErrorEntity ValidationError { get; set; } 
    }
}