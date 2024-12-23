namespace Application.UseCases.Commands.Order
{
    public class CreateOrderCommand : IRequest<IResult>
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public string? UserId { get; set; }
        public string NonceFromClient { get; set; }
        public CreateOrderRequest Request { get; set; }
        public CreateOrderCommand(int hotelId, int number, string? userId, string nonceFromClient, CreateOrderRequest request)
        {
            HotelId = hotelId;
            Number = number;
            UserId = userId;
            Request = request;
            NonceFromClient = nonceFromClient;
        }
    }
}
