using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Core.Interfaces;
using Core.TasksResults;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateHotelCommand> _validator;
        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateHotelCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<IResult> Handle(UpdateHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = _validator.Validate(command);

                if (!validationResult.IsValid)
                {
                    return new Fail(validationResult.ToDictionary().ToString()!, Core.Enums.ErrorStatuses.ValidationError);
                }

                var result = await _unitOfWork.Hotels.Update(command.Id, command.Request);

                if (!result.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return result;
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the hotel.", ex);
            }
        }
    }
}
