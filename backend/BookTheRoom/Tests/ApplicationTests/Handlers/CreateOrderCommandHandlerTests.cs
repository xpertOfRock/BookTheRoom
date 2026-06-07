using Application.Interfaces;
using Application.UseCases.Commands.Order;
using Application.UseCases.Handlers.CommandHandlers.Order;
using Braintree;
using Core.Contracts;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.TasksResults;
using Address = Core.ValueObjects.Address;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for CreateOrderCommandHandler.
    // Covers: null-check fix (room.Result/hotel.Result), payment failure path,
    // conflict-error path, and the happy path that returns the new order id.
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHotelRepository> _hotels = new();
        private readonly Mock<IRoomRepository> _rooms = new();
        private readonly Mock<IOrderRepository> _orders = new();
        private readonly Mock<IPaymentService> _paymentService = new();
        private readonly Mock<IEmailService> _emailService = new();
        private readonly Mock<IBraintreeGateway> _gateway = new();
        private readonly Mock<ITransactionGateway> _transactionGateway = new();
        private readonly CreateOrderCommandHandler _handler;

        private static readonly Hotel SampleHotel = new Hotel
        {
            Id = 1,
            Name = "Test Hotel",
            Description = "desc",
            Rating = 4,
            HasPool = false,
            Address = new Address("Ukraine", "Kyiv Oblast", "Kyiv", "Main St 1", "01001"),
            Images = new List<string>(),
            Comments = new List<Comment>()
        };

        private static readonly Core.Entities.Room SampleRoom = new Core.Entities.Room
        {
            HotelId = 1,
            Number = 101,
            Name = "Standard",
            Description = "desc",
            Price = 100m,
            Category = RoomCategory.OneBedApartments,
            Images = new List<string>()
        };

        public CreateOrderCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Hotels).Returns(_hotels.Object);
            _unitOfWork.Setup(u => u.Rooms).Returns(_rooms.Object);
            _unitOfWork.Setup(u => u.Orders).Returns(_orders.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _gateway.Setup(g => g.Transaction).Returns(_transactionGateway.Object);
            _paymentService.Setup(p => p.CreateGateway()).Returns(_gateway.Object);
            _emailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(),
                                                  It.IsAny<string>()));

            _handler = new CreateOrderCommandHandler(
                _unitOfWork.Object, _paymentService.Object, _emailService.Object);
        }

        private static CreateOrderCommand BuildCommand(int days = 3)
        {
            // Capture a single UtcNow to avoid sub-millisecond drift between
            // CheckIn and CheckOut causing Math.Ceiling to round up an extra day.
            var now = DateTimeOffset.UtcNow;
            return new CreateOrderCommand(
                hotelId: 1,
                number: 101,
                userId: "user-001",
                request: new CreateOrderRequest(
                    NonceFromClient: "fake-nonce",
                    Email: "guest@example.com",
                    Number: "+380000000000",
                    FirstName: "John",
                    LastName: "Doe",
                    MinibarIncluded: false,
                    MealsIncluded: false,
                    CheckIn: now.AddDays(1),
                    CheckOut: now.AddDays(1 + days)));
        }

        private void SetupHotelAndRoom()
        {
            _hotels.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(SampleHotel);
            _rooms.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(SampleRoom);
        }

        private void SetupPaymentSuccess()
        {
            var successResult = new Mock<Result<Transaction>>();
            successResult.Setup(r => r.IsSuccess()).Returns(true);
            _transactionGateway
                .Setup(t => t.SaleAsync(It.IsAny<TransactionRequest>()))
                .ReturnsAsync(successResult.Object);
        }

        private void SetupPaymentFailure()
        {
            var failResult = new Mock<Result<Transaction>>();
            failResult.Setup(r => r.IsSuccess()).Returns(false);
            _transactionGateway
                .Setup(t => t.SaleAsync(It.IsAny<TransactionRequest>()))
                .ReturnsAsync(failResult.Object);
        }

        [Fact]
        public async Task Handle_HappyPath_CommitsAndReturnsOrderId()
        {
            SetupHotelAndRoom();
            _orders.Setup(o => o.Add(It.IsAny<Core.Entities.Order>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new Success("order created"));
            SetupPaymentSuccess();

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenHotelIsNull_RollsBackAndReturnsNotFound()
        {
            _hotels.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((Hotel?)null);
            _rooms.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(SampleRoom);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRoomIsNull_RollsBackAndReturnsNotFound()
        {
            _hotels.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(SampleHotel);
            _rooms.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync((Core.Entities.Room?)null);

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPaymentFails_RollsBackAndReturnsFail()
        {
            SetupHotelAndRoom();
            _orders.Setup(o => o.Add(It.IsAny<Core.Entities.Order>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new Success("order added"));
            SetupPaymentFailure();

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOrderAddFails_ConflictOrFail_RollsBack()
        {
            SetupHotelAndRoom();
            _orders.Setup(o => o.Add(It.IsAny<Core.Entities.Order>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new Fail("Room is already booked.", ErrorStatuses.ConflictError));

            // Payment should not even be attempted.
            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _transactionGateway.Verify(
                t => t.SaleAsync(It.IsAny<TransactionRequest>()), Times.Never);
        }

        [Fact]
        public async Task Handle_CalculatesOverallPriceCorrectly()
        {
            SetupHotelAndRoom();

            Core.Entities.Order? captured = null;
            _orders.Setup(o => o.Add(It.IsAny<Core.Entities.Order>(), It.IsAny<CancellationToken>()))
                   .Callback<Core.Entities.Order, CancellationToken>((o, _) => captured = o)
                   .ReturnsAsync(new Success("ok"));
            SetupPaymentSuccess();

            // 3 days * 100 (room price) * 1.05 (base coefficient) = 315
            await _handler.Handle(BuildCommand(days: 3), CancellationToken.None);

            Assert.NotNull(captured);
            Assert.Equal(315m, captured!.OverallPrice);
        }

        [Fact]
        public async Task Handle_WhenMealsAndMinibarIncluded_IncreasesPrice()
        {
            SetupHotelAndRoom();

            Core.Entities.Order? captured = null;
            _orders.Setup(o => o.Add(It.IsAny<Core.Entities.Order>(), It.IsAny<CancellationToken>()))
                   .Callback<Core.Entities.Order, CancellationToken>((o, _) => captured = o)
                   .ReturnsAsync(new Success("ok"));
            SetupPaymentSuccess();

            var baseTime = DateTimeOffset.UtcNow;
            var command = new CreateOrderCommand(1, 101, "user-001",
                new CreateOrderRequest("nonce", "a@b.com", "+1", "A", "B",
                    MinibarIncluded: true, MealsIncluded: true,
                    CheckIn: baseTime.AddDays(1),
                    CheckOut: baseTime.AddDays(4)));

            await _handler.Handle(command, CancellationToken.None);

            // 3 days * 100 * (1.05 + 0.03 + 0.03) = 333
            Assert.NotNull(captured);
            Assert.Equal(333m, captured!.OverallPrice);
        }
    }
}
