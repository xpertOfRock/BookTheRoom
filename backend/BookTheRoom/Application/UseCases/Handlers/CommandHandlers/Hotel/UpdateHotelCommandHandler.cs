using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateHotelCommand command, CancellationToken cancellationToken)
        {       
            await _unitOfWork.Hotels.Update(command.Id, command.UpdateHotelRequest);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
