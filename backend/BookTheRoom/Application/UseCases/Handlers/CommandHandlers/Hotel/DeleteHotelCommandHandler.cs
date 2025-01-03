using Application.UseCases.Abstractions;
using Application.UseCases.Commands.Hotel;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class DeleteHotelCommandHandler : ICommandHandler<DeleteHotelCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(DeleteHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await _unitOfWork.Hotels.Delete(command.Id);

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
