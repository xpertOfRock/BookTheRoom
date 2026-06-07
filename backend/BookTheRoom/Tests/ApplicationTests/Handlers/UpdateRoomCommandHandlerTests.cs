using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Application.UseCases.Handlers.CommandHandlers.Room;
using Core.Contracts;
using Core.Enums;
using Core.Interfaces;
using Core.TasksResults;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for UpdateRoomCommandHandler.
    public class UpdateRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IRoomRepository> _rooms = new();
        private readonly UpdateRoomCommandHandler _handler;

        private static readonly UpdateRoomRequest DefaultRequest = new UpdateRoomRequest(
            Name: "Deluxe",
            Description: "Upgraded room",
            Price: 200m,
            Category: RoomCategory.TwoBedApartments,
            Images: null);

        public UpdateRoomCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Rooms).Returns(_rooms.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new UpdateRoomCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WhenUpdateSucceeds_CommitsAndReturnsSuccess()
        {
            _rooms.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<int>(),
                                       It.IsAny<UpdateRoomRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Success("Room updated."));

            var result = await _handler.Handle(
                new UpdateRoomCommand(1, 101, DefaultRequest), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRoomNotFound_RollsBackAndReturnsFail()
        {
            _rooms.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<int>(),
                                       It.IsAny<UpdateRoomRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Fail("Room not found."));

            var result = await _handler.Handle(
                new UpdateRoomCommand(1, 999, DefaultRequest), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _rooms.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<int>(),
                                       It.IsAny<UpdateRoomRequest>(), It.IsAny<CancellationToken>()))
                  .ThrowsAsync(new Exception("db error"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(new UpdateRoomCommand(1, 101, DefaultRequest), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesCorrectArgumentsToRepository()
        {
            _rooms.Setup(r => r.Update(It.IsAny<int>(), It.IsAny<int>(),
                                       It.IsAny<UpdateRoomRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Success("ok"));

            await _handler.Handle(new UpdateRoomCommand(hotelId: 3, number: 202, DefaultRequest),
                                  CancellationToken.None);

            _rooms.Verify(r => r.Update(3, 202, DefaultRequest, It.IsAny<CancellationToken>()),
                          Times.Once);
        }
    }
}
