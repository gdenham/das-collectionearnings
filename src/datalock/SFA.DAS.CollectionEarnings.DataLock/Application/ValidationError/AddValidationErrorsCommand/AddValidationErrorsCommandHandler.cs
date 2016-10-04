using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandHandler : IRequestHandler<AddValidationErrorsCommandRequest, Unit>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorsCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public Unit Handle(AddValidationErrorsCommandRequest message)
        {
            _validationErrorRepository.AddValidationErrors(message.ValidationErrors);

            return Unit.Value;
        }
    }
}