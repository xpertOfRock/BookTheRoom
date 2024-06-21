using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Order
{
    public class CreateOrderCommand : IRequest<Unit>
    {
        public Core.Entities.Order Order { get; set; }
        public CreateOrderCommand(Core.Entities.Order order)
        {
            Order = order;
        }
    }
}
