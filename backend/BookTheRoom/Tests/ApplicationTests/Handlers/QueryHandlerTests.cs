using Application.Interfaces;
using Application.UseCases.Handlers.QueryHandlers.Hotel;
using Application.UseCases.Handlers.QueryHandlers.Room;
using Application.UseCases.Queries.Hotel;
using Application.UseCases.Queries.Room;
using Core.Contracts;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for thin query handlers that delegate straight to repositories.
    public class QueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHotelRepository> _hotels = new();
        private readonly Mock<IRoomRepository> _rooms = new();
        private readonly Mock<IOrderRepository> _orders = new();

        public QueryHandlerTests()
        {
            _unitOfWork.Setup(u => u.Hotels).Returns(_hotels.Object);
            _unitOfWork.Setup(u => u.Rooms).Returns(_rooms.Object);
            _unitOfWork.Setup(u => u.Orders).Returns(_orders.Object);
        }

        private static Hotel MakeHotel(int id = 1) => new Hotel
        {
            Id = id,
            Name = "Test Hotel",
            Description = "desc",
            Rating = 4,
            HasPool = false,
            Address = new Address("Ukraine", "Kyiv Oblast", "Kyiv", "Main St 1", "01001"),
            Images = new List<string>(),
            Comments = new List<Comment>()
        };

        // GetHotelQueryHandler

        [Fact]
        public async Task GetHotelQuery_ReturnsHotelFromRepository()
        {
            var hotel = MakeHotel(id: 5);
            _hotels.Setup(r => r.GetById(5, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(hotel);

            var handler = new GetHotelQueryHandler(_unitOfWork.Object);
            var result = await handler.Handle(new GetHotelQuery(5), CancellationToken.None);

            Assert.Equal(hotel, result);
        }

        [Fact]
        public async Task GetHotelQuery_CallsRepositoryWithCorrectId()
        {
            _hotels.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(MakeHotel());

            var handler = new GetHotelQueryHandler(_unitOfWork.Object);
            await handler.Handle(new GetHotelQuery(42), CancellationToken.None);

            _hotels.Verify(r => r.GetById(42, It.IsAny<CancellationToken>()), Times.Once);
        }

        // GetHotelsQueryHandler

        [Fact]
        public async Task GetHotelsQuery_ReturnsAllHotels()
        {
            var hotels = new List<Hotel> { MakeHotel(1), MakeHotel(2) };
            _hotels.Setup(r => r.GetAll(It.IsAny<GetHotelsRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(hotels);

            var handler = new GetHotelsQueryHandler(_unitOfWork.Object);
            var filter = new GetHotelsRequest(null, null, null, null, null, null, null);
            var result = await handler.Handle(new GetHotelsQuery(filter), CancellationToken.None);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetHotelsQuery_PassesFilterToRepository()
        {
            _hotels.Setup(r => r.GetAll(It.IsAny<GetHotelsRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<Hotel>());

            var filter = new GetHotelsRequest("Kyiv", null, "desc", null, null, null, null);
            var handler = new GetHotelsQueryHandler(_unitOfWork.Object);
            await handler.Handle(new GetHotelsQuery(filter), CancellationToken.None);

            _hotels.Verify(r => r.GetAll(
                It.Is<GetHotelsRequest>(f => f.Search == "Kyiv"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        // GetHotelRoomsQueryHandler — includes availability filtering

        [Fact]
        public async Task GetHotelRoomsQuery_WhenNoOrders_ReturnsAllRooms()
        {
            var checkIn = DateTime.UtcNow.Date.AddDays(1);
            var checkOut = checkIn.AddDays(2);

            var rooms = new List<Core.Entities.Room>
            {
                new Core.Entities.Room { HotelId = 1, Number = 101, Name = "Std", Category = RoomCategory.OneBedApartments, Images = new List<string>(), Price = 100m, Description = "d" },
                new Core.Entities.Room { HotelId = 1, Number = 102, Name = "Dlx", Category = RoomCategory.TwoBedApartments, Images = new List<string>(), Price = 150m, Description = "d" }
            };

            _rooms.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<GetRoomsRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(rooms);
            _orders.Setup(o => o.GetAll(It.IsAny<GetOrdersRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<Core.Entities.Order>());

            var mockValidator = new Mock<IValidator<GetHotelRoomsQuery>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<GetHotelRoomsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());

            var handler = new GetHotelRoomsQueryHandler(_unitOfWork.Object, mockValidator.Object);
            var query = new GetHotelRoomsQuery(1, new GetRoomsRequest(
                null, null, null, null, 0m, 500m, checkIn, checkOut));

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetHotelRoomsQuery_WhenRoomIsBooked_ExcludesIt()
        {
            var checkIn = DateTime.UtcNow.Date.AddDays(2);
            var checkOut = checkIn.AddDays(3);

            var rooms = new List<Core.Entities.Room>
            {
                new Core.Entities.Room { HotelId = 1, Number = 101, Name = "Std", Category = RoomCategory.OneBedApartments, Images = new List<string>(), Price = 100m, Description = "d" },
                new Core.Entities.Room { HotelId = 1, Number = 102, Name = "Dlx", Category = RoomCategory.TwoBedApartments, Images = new List<string>(), Price = 150m, Description = "d" }
            };

            // Room 101 is occupied during the requested period.
            var existingOrder = new Core.Entities.Order
            {
                HotelId = 1,
                RoomNumber = 101,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Status = OrderStatus.Awaiting
            };

            _rooms.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<GetRoomsRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(rooms);
            _orders.Setup(o => o.GetAll(It.IsAny<GetOrdersRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<Core.Entities.Order> { existingOrder });

            var mockValidator = new Mock<IValidator<GetHotelRoomsQuery>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<GetHotelRoomsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());

            var handler = new GetHotelRoomsQueryHandler(_unitOfWork.Object, mockValidator.Object);
            var query = new GetHotelRoomsQuery(1, new GetRoomsRequest(
                null, null, null, null, 0m, 500m, checkIn, checkOut));

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(102, result[0].Number);
        }

        [Fact]
        public async Task GetHotelRoomsQuery_WhenValidationFails_ThrowsValidationException()
        {
            var mockValidator = new Mock<IValidator<GetHotelRoomsQuery>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<GetHotelRoomsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult(
                             new[] { new ValidationFailure("CheckIn", "CheckIn value must be greater than today") }));

            var handler = new GetHotelRoomsQueryHandler(_unitOfWork.Object, mockValidator.Object);
            var query = new GetHotelRoomsQuery(1, new GetRoomsRequest(
                null, null, null, null, 0m, 500m,
                DateTime.UtcNow.Date,           // invalid: today, not tomorrow
                DateTime.UtcNow.Date.AddDays(2)));

            await Assert.ThrowsAsync<ValidationException>(
                () => handler.Handle(query, CancellationToken.None));
        }
    }
}
