using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateRoomCommand> _validator;
        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateRoomCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = _validator.Validate(command);

                if (!validationResult.IsValid)
                {
                    return new Fail("Validation is failed.", ErrorStatuses.ValidationError);
                }

                var result = await _unitOfWork.Rooms.Update(command.HotelId, command.Number, command.Request);

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
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
        }
    }
}
