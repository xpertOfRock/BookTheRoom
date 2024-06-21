using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Order
{
    public class UpdateOrderCommand : IRequest<Unit>
    {
        public Core.Entities.Order Order { get; set; }
        public UpdateOrderCommand(Core.Entities.Order order)
        {
            Order = order;
        }
    }
}
