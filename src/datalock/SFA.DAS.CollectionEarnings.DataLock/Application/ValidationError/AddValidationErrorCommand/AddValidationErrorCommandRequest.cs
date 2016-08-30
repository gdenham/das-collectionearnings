using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandRequest : IRequest<AddValidationErrorCommandResponse>
    {
        public Data.Entities.ValidationError ValidationError { get; set; } 
    }
}