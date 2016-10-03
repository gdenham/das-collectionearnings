using System;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.ValidationError.AddValidationErrorsCommand
{
    public class AddValidationErrorsCommandHandler : IRequestHandler<AddValidationErrorsCommandRequest, AddValidationErrorsCommandResponse>
    {
        private readonly IValidationErrorRepository _validationErrorRepository;

        public AddValidationErrorsCommandHandler(IValidationErrorRepository validationErrorRepository)
        {
            _validationErrorRepository = validationErrorRepository;
        }

        public AddValidationErrorsCommandResponse Handle(AddValidationErrorsCommandRequest message)
        {
            try
            {
                _validationErrorRepository.AddValidationErrors(message.ValidationErrors);

                return new AddValidationErrorsCommandResponse
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new AddValidationErrorsCommandResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}