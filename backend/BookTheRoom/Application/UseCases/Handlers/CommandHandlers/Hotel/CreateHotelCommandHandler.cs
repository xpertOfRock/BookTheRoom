using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.Hotels.Add
                (
                    new Core.Entities.Hotel
                    {
                        Name = command.Name,
                        Description = command.Description,
                        Rating = command.Rating,
                        HasPool = command.HasPool,
                        Address = command.Address,
                        Images = command.Images,
                        Comments = new List<Core.Entities.Comment>()
                    }
                );

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the hotel.", ex);
            }

            return Unit.Value;
        }
    }
}
