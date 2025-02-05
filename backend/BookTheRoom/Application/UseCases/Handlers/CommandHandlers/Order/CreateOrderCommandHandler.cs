using Application.UseCases.Commands.Order;
using Braintree;
using Core.Entities;

namespace Application.UseCases.Handlers.CommandHandlers.Order
{
    public class CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IEmailService emailService) 
        : ICommandHandler<CreateOrderCommand, IResult>
    {
        public async Task<IResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var hotel = await unitOfWork.Hotels.GetById(command.HotelId, cancellationToken);

                if (hotel is null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Fail("[Post Order] Hotel is null.", ErrorStatuses.NotFoundError);
                }

                var room = await unitOfWork.Rooms.GetById(command.HotelId, command.Number, cancellationToken);

                if (room is null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Fail("[Post Order] Room is null", ErrorStatuses.NotFoundError);
                }

                var duration = command.Request.CheckOut.Subtract(command.Request.CheckIn);
                int days = (int)Math.Ceiling(duration.TotalDays);

                decimal price = room!.Price;

                decimal multiplier = 1.05m;

                if (command.Request.MealsIncluded) multiplier += 0.03m;
                if (command.Request.MinibarIncluded) multiplier += 0.03m;

                var overallPrice = days * price * multiplier;

                var order = new Core.Entities.Order
                {
                    Email = command.Request.Email,
                    Phone = command.Request.Number,
                    FirstName = command.Request.FirstName,
                    LastName = command.Request.LastName,
                    CreatedAt = DateTime.UtcNow,
                    CheckIn = command.Request.CheckIn.Date.ToUniversalTime(),
                    CheckOut = command.Request.CheckOut.ToUniversalTime(),
                    OverallPrice = overallPrice,
                    UserId = command.UserId,
                    HotelId = command.HotelId,
                    RoomNumber = command.Number,
                    MealsIncluded = command.Request.MealsIncluded,
                    MinibarIncluded = command.Request.MinibarIncluded,
                    Status = OrderStatus.Awaiting
                };

                await unitOfWork.Orders.Add(order);

                await unitOfWork.SaveChangesAsync();

                var paymentResult = await ProcessPayment(order.Id.ToString(), order.OverallPrice, command.Request.NonceFromClient);

                if(!paymentResult.IsSuccess)
                {
                    await unitOfWork.RollbackAsync();
                    return paymentResult;
                }               

                await unitOfWork.CommitAsync();

                PostMessage(command.Request.Email, order, hotel, days);

                return new Success(order.Id.ToString());
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the order.", ex);
            }
        }

        private void PostMessage(string emailToSend, Core.Entities.Order order, Core.Entities.Hotel hotel, int days)
        {
            string subject = $"Order No. {order.Id}";

            string body = "Thanks for choosing Book The Room!\n\n" +

                         $"Your hotel: {hotel.Name} ( Address: {hotel.Address.ToString()} )\n" +
                         $"You have successfully booked the room with immediate payment!\n" +
                         $"Room No. : {order.RoomNumber}\n" +
                         $"Duration: {days} days\n" +
                         $"Check in date: {order.CheckIn.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Check out date: {order.CheckOut.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Overall price : {Math.Round(order.OverallPrice, 2)}\n\n" +
                         $"Have a nice day!";

            emailService.SendEmail(emailToSend, subject, body);
        }

        private async Task<IResult> ProcessPayment(string orderId, decimal price, string nonceFromClient)
        {
            var gateway = paymentService.CreateGateway();

            var request = new TransactionRequest
            {
                Amount = price,
                OrderId = orderId,
                PaymentMethodNonce = nonceFromClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var transactionResult = await gateway.Transaction.SaleAsync(request);

            IResult result = transactionResult.IsSuccess() 
                ? new Success("[Post Order] Paymemt successfully processed & order success")
                : new Fail("[Post order] Card details are invalid or not enough funds on given card.", ErrorStatuses.ValidationError);

            return result;         
        }
    }
}