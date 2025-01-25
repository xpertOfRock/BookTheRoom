using Application.UseCases.Commands.Room;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class CreateRoomCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateRoomCommand, IResult>
    {
        public async Task<IResult> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var addResult = await unitOfWork.Rooms.Add(
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
                    await unitOfWork.RollbackAsync();
                    return result;
                }
                
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
        }
    }
}
