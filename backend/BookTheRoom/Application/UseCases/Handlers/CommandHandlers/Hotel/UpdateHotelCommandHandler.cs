using Application.UseCases.Commands.Hotel;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class UpdateHotelCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateHotelCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateHotelCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await unitOfWork.Hotels.Update(command.Id, command.Request);

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
                throw new InvalidOperationException("An error occurred while processing the hotel.", ex);
            }
        }
    }
}
