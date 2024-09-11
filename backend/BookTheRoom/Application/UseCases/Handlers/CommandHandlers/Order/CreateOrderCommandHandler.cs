using Application.Interfaces;
using Application.UseCases.Commands.Order;
using Braintree;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Order
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IPaymentService _paymentService;
        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _emailService = emailService;

        }
        public async Task<Unit> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var hotel = await _unitOfWork.Hotels.GetById(command.HotelId);
            var room = await _unitOfWork.Rooms.GetById(command.HotelId, command.Number);

            var duration = command.Request.CheckOut.Subtract(command.Request.CheckIn);

            int days = (int)Math.Ceiling(duration.TotalDays);

            var price = room.Price;

            if(command.Request.MealsIncluded is true)
            {
                price *= 1.1m;
            }

            if(command.Request.MinibarIncluded is true)
            {
                price *= 1.1m;
            }

            var overallPrice = days * price;

            var gateway = _paymentService.CreateGateway();
            var clientToken = gateway.ClientToken.Generate();

            string nonceFromClient = command.Request.NonceFromClient;

            var order = new Core.Entities.Order
            {
                Email = command.Request.Email,
                Phone = command.Request.Number,
                CreatedAt = DateTime.UtcNow,
                CheckIn = command.Request.CheckIn,
                CheckOut = command.Request.CheckOut,
                MealsIncluded = command.Request.MealsIncluded,
                MinibarIncluded = command.Request.MinibarIncluded,
                OverallPrice = overallPrice,
                UserId = command.UserId,
                HotelId = command.HotelId,
                RoomId = command.Number,
                Hotel = hotel,
                Room = room
            };

            var request = new TransactionRequest
            {
                Amount = order.OverallPrice,
                OrderId = order.Id.ToString(),
                PaymentMethodNonce = nonceFromClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = await gateway.Transaction.SaleAsync(request);

            if (result.Target.ProcessorResponseText == "Approved")
            {
                await _unitOfWork.Orders.Add(order);

                string subject = $"Order No. {order.Id}";
                string body = "Thanks for choosing Book The Room!\n\n" +
                             $"Your hotel: {order.Hotel.Name}\n" +
                             $"Address: {order.Hotel.Address.Country}," +
                                $" {order.Hotel.Address.State}," +
                                $" {order.Hotel.Address.City}," +
                                $" {order.Hotel.Address.Street}," +
                                $" {order.Hotel.Address.PostalCode}\n" +
                             $"Room No. : {order.Room.Number}\n" +
                             $"Duration: {days} days\n" +
                             $"Check in date: {order.CheckIn.ToString("dd.MM.yyyy HH:mm")}\n" +
                             $"Check out date: {order.CheckOut.ToString("dd.MM.yyyy HH:mm")}\n" +
                             $"Overall price : {Math.Round(order.OverallPrice, 2)}\n\n" +
                             $"Have a nice day!";
                
                _emailService.SendEmail(order.Email, subject, body);
                
                await _unitOfWork.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
