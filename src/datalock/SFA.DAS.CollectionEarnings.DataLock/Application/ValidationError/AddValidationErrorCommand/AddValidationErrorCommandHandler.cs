using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandHandler : IRequestHandler<AddValidationErrorCommandRequest, Unit>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public Unit Handle(AddValidationErrorCommandRequest message)
        {
            _validationErrorRepository.AddValidationError(message.ValidationError);

            return Unit.Value;
        }
    }
}