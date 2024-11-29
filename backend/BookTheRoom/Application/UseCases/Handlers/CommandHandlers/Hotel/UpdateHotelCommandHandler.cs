using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Core.Interfaces;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(UpdateHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.Hotels.Update(command.Id, command.UpdateHotelRequest);

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
