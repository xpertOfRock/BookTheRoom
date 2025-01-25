using Application.UseCases.Commands.Order;

namespace Application.UseCases.Handlers.CommandHandlers.Order
{
    public class UpdateOrderCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {               
                await unitOfWork.Orders.Update(request.Id, request.Request);

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the order.", ex);
            }
            return Unit.Value;
        }
    }
}
