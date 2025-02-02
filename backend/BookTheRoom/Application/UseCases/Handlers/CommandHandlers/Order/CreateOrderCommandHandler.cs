using Application.UseCases.Commands.Order;
using Braintree;

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
                var hotel = await unitOfWork.Hotels.GetById(command.HotelId);
                var room = await unitOfWork.Rooms.GetById(command.HotelId, command.Number);

                var duration = command.Request.CheckOut.Subtract(command.Request.CheckIn);
                int days = (int)Math.Ceiling(duration.TotalDays);

                decimal price = room.Price;

                decimal multiplier = 1.0m;

                if (command.Request.MealsIncluded) multiplier += 0.1m;
                if (command.Request.MinibarIncluded) multiplier += 0.1m;

                var overallPrice = days * price * multiplier;

                var order = new Core.Entities.Order
                {
                    Email = command.Request.Email,
                    Phone = command.Request.Number,
                    CreatedAt = DateTime.UtcNow,
                    CheckIn = command.Request.CheckIn,
                    CheckOut = command.Request.CheckOut,
                    OverallPrice = overallPrice,
                    IsPaid = false,
                    UserId = command.UserId,
                    HotelId = command.HotelId,
                    RoomId = command.Number,
                    Hotel = hotel,
                    Room = room
                };

                

                if (command.Request.PaidImmediately)
                {
                    var gateway = paymentService.CreateGateway();
                    var request = new TransactionRequest
                    {
                        Amount = order.OverallPrice,
                        OrderId = order.Id.ToString(),
                        PaymentMethodNonce = command.Request.NonceFromClient,
                        Options = new TransactionOptionsRequest
                        {
                            SubmitForSettlement = true
                        }
                    };

                    var transactionResult = await gateway.Transaction.SaleAsync(request);

                    if (!transactionResult.IsSuccess() || transactionResult.Target.ProcessorResponseText != "Approved")
                    {
                        await unitOfWork.RollbackAsync();
                        return new Fail("Card details are invalid or not enough funds on given card.");
                    }
                    order.IsPaid = true;                  
                }

                var result =  await unitOfWork.Orders.Add(order);

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                SendMail(order.Email, order);

                return result;            
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the order.", ex);
            }
        }   
        
        private void SendMail(string email, Core.Entities.Order order)
        {
            var duration = order.CheckOut.Subtract(order.CheckIn);

            int days = (int)Math.Ceiling(duration.TotalDays);

            string subject = $"Order No. {order.Id}";

            string isPaidInformation = string.Empty;

            switch (order.IsPaid) 
            {
                case true:
                    isPaidInformation = $"You have successfully booked the room with immediate payment!\n" +
                                        $"In case you want to cancel this reservation and return your money, please contact customer service.\n";
                    break;
                case false:
                    isPaidInformation = $"Attention!\nYou successfully booked the room without immediate payment!\n" +
                                        $"After arriving at your hotel you will have to pay your reservation at the hotel reception.\n";
                    break;
            }

            string body = "Thanks for choosing Book The Room!\n\n" +
                         
                         $"Your hotel: {order.Hotel.Name} ( Address: {order.Hotel.Address.ToString()} )\n {isPaidInformation}" +                           
                         $"Room No. : {order.Room.Number}\n" +
                         $"Duration: {days} days\n" +
                         $"Check in date: {order.CheckIn.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Check out date: {order.CheckOut.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Overall price : {Math.Round(order.OverallPrice, 2)}\n\n" +
                         $"Have a nice day!";

            emailService.SendEmail(email, subject, body);
        }
    }
}