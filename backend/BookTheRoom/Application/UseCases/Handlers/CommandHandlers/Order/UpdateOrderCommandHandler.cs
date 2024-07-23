using Application.Interfaces;
using Application.UseCases.Commands.Order;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Order
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Orders.Update(request.Id, request.Request);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
