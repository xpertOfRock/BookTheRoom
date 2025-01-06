using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateRoomCommand> _validator;
        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateRoomCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Fail("Validation is failed.", ErrorStatuses.ValidationError);
                }

                var addResult = await _unitOfWork.Rooms.Add(
                    new Core.Entities.Room
                    {
                        Name = command.Name,
                        Description = command.Description,
                        Number = command.Number,
                        Price = command.Price,
                        Category = command.Category,
                        HotelId = command.HotelId,
                        Images = command.Images
                    }
                );

                IResult result = !addResult.IsSuccess
                               ? new Fail("Could not add room to the list of hotel rooms.")
                               : new Success("Room was successfuly added to the list of hotel rooms.");

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
