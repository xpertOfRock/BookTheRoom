using Application.UseCases.Abstractions;

namespace Application.UseCases.Commands.Order
{
    public class UpdateOrderCommand : ICommand<Unit>
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
