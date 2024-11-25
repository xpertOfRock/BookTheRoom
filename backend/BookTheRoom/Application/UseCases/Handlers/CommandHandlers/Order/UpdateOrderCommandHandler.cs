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
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.Orders.Update(request.Id, request.Request);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the order.", ex);
            }
            return Unit.Value;
        }
    }
}
