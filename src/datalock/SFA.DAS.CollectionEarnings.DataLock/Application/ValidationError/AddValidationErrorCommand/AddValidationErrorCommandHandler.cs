using System;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorCommand
{
    public class AddValidationErrorCommandHandler : IRequestHandler<AddValidationErrorCommandRequest, AddValidationErrorCommandResponse>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public AddValidationErrorCommandResponse Handle(AddValidationErrorCommandRequest message)
        {
            try
            {
                _validationErrorRepository.AddValidationError(message.ValidationError);

                return new AddValidationErrorCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddValidationErrorCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}