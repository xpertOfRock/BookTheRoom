using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Application.UseCases.Handlers.CommandHandlers.Room;
using Core.Contracts;
using Core.Enums;
using Core.Interfaces;
using Core.TasksResults;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for CreateRoomCommandHandler.
    // The handler wraps Add in a transaction; commit on success, rollback on failure.
    public class CreateRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IRoomRepository> _rooms = new();
        private readonly CreateRoomCommandHandler _handler;

        public CreateRoomCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Rooms).Returns(_rooms.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new CreateRoomCommandHandler(_unitOfWork.Object);
        }

        private static CreateRoomCommand BuildCommand(int hotelId = 1, int number = 101) =>
            new CreateRoomCommand(hotelId, new CreateRoomRequest(
                Name: "Standard",
                Description: "A comfortable room",
                Number: number,
                Price: 120m,
                Category: RoomCategory.OneBedApartments,
                Images: new List<string>()));

        [Fact]
        public async Task Handle_WhenAddSucceeds_CommitsAndReturnsSuccess()
        {
            _rooms.Setup(r => r.Add(It.IsAny<Core.Entities.Room>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Success("Room was successfully added to the list of hotel rooms."));

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenAddFails_RollsBackAndReturnsFail()
        {
            _rooms.Setup(r => r.Add(It.IsAny<Core.Entities.Room>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Fail("Could not add room to the list of hotel rooms."));

            var result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _rooms.Setup(r => r.Add(It.IsAny<Core.Entities.Room>(), It.IsAny<CancellationToken>()))
                  .ThrowsAsync(new Exception("db failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(BuildCommand(), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_MapsCommandFieldsToRoom()
        {
            Core.Entities.Room? capturedRoom = null;

            _rooms.Setup(r => r.Add(It.IsAny<Core.Entities.Room>(), It.IsAny<CancellationToken>()))
                  .Callback<Core.Entities.Room, CancellationToken>((room, _) => capturedRoom = room)
                  .ReturnsAsync(new Success("ok"));

            var command = BuildCommand(hotelId: 7, number: 205);
            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedRoom);
            Assert.Equal(7, capturedRoom!.HotelId);
            Assert.Equal(205, capturedRoom.Number);
            Assert.Equal("Standard", capturedRoom.Name);
            Assert.Equal(120m, capturedRoom.Price);
        }
    }
}
