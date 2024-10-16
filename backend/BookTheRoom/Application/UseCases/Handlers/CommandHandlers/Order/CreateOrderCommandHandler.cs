//using Application.Interfaces;
//using Application.UseCases.Commands.Order;
//using Braintree;
//using Core.Contracts;
//using Core.Enums;
//using MediatR;

//namespace Application.UseCases.Handlers.CommandHandlers.Order
//{
//    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IPaymentService _paymentService;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, UserManager<ApplicationUser> userManager)
//        {
//            _unitOfWork = unitOfWork;
//            _paymentService = paymentService;
//            _userManager = userManager;
//        }

//        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
//        {
//            var hotel = await _unitOfWork.Hotels.GetById(request.HotelId);
//            var room = hotel.Rooms.FirstOrDefault(r => r.Number == request.Number);

//            if (hotel == null || room == null)
//            {
//                throw new Exception("Hotel or Room not found");
//            }

//            decimal overallPrice = room.Price * (request.Request.CheckOut - request.Request.CheckIn).Days;
//            if (request.Request.MinibarIncluded)
//            {
//                overallPrice += 50;
//            }
//            if (request.Request.MealsIncluded)
//            {
//                overallPrice += 100;
//            }

//            var newOrder = new Core.Entities.Order
//            {
//                HotelId = request.HotelId,
//                RoomNumber = request.Number,
//                Email = request.Request.Email,
//                Phone = request.Request.Number,
//                MinibarIncluded = request.Request.MinibarIncluded,
//                MealsIncluded = request.Request.MealsIncluded,
//                IsPaid = false,
//                CheckIn = request.Request.CheckIn,
//                CheckOut = request.Request.CheckOut,
//                CreatedAt = request.Request.CreatedAt,
//                Status = OrderStatus.Pending,
//                OverallPrice = overallPrice,
//            };

//            var gateway = _paymentService.GetGateway();
//            var requestPayment = new Braintree.TransactionRequest
//            {
//                Amount = overallPrice,
//                PaymentMethodNonce = "fake-valid-nonce",
//                Options = new Braintree.TransactionOptionsRequest
//                {
//                    SubmitForSettlement = true
//                }
//            };

//            var result = await gateway.Transaction.SaleAsync(requestPayment);

//            if (result.IsSuccess())
//            {
//                newOrder.IsPaid = true;
//                newOrder.Status = OrderStatus.Active;
//            }
//            else
//            {
//                throw new Exception("Payment failed: " + result.Message);
//            }

//            if (!string.IsNullOrEmpty(request.UserId))
//            {
//                var user = await _userManager.FindByIdAsync(request.UserId);
//                if (user != null)
//                {
//                    user.Orders ??= new List<Core.Entities.Order>();
//                    user.Orders.Add(newOrder);

//                    await _userManager.UpdateAsync(user);
//                }
//            }

//            await _unitOfWork.Orders.Add(newOrder);
//            await _unitOfWork.SaveChangesAsync();

//            return Unit.Value;
//        }
    

//    private string GenerateOrderEmailBody(Core.Entities.Order order, int days)
//        {
//            return "Thanks for choosing Book The Room!\n\n" +
//                   $"Your hotel: {order.Hotel.Name}\n" +
//                   $"Room No. : {order.RoomNumber}\n" +
//                   $"Duration: {days} days\n" +
//                   $"Check in date: {order.CheckIn:dd.MM.yyyy HH:mm}\n" +
//                   $"Check out date: {order.CheckOut:dd.MM.yyyy HH:mm}\n" +
//                   $"Overall price : {Math.Round(order.OverallPrice, 2)}\n\n" +
//                   "Have a nice day!";
//        }
//    }

//}
