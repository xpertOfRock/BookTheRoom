using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Hotels.Delete(command.Id);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
