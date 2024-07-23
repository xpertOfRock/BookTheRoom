using Core.Contracts;
using Core.Enums;
using MediatR;

namespace Application.UseCases.Commands.Order
{
    public class UpdateOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateOrderRequest Request { get; set; }
        public UpdateOrderCommand(int id, UpdateOrderRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
