using Application.UseCases.Commands.Hotel;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class DeleteHotelCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteHotelCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteHotelCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await unitOfWork.Hotels.Delete(command.Id);

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
