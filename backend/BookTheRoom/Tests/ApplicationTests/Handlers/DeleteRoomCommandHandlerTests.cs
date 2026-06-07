using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Application.UseCases.Handlers.CommandHandlers.Room;
using Core.Interfaces;
using Core.TasksResults;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for DeleteRoomCommandHandler.
    public class DeleteRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IRoomRepository> _rooms = new();
        private readonly DeleteRoomCommandHandler _handler;

        public DeleteRoomCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Rooms).Returns(_rooms.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new DeleteRoomCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_CommitsAndReturnsSuccess()
        {
            _rooms.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Success("Room was deleted."));

            var result = await _handler.Handle(new DeleteRoomCommand(1, 101), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRoomNotFound_RollsBackAndReturnsFail()
        {
            _rooms.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Fail("Room not found."));

            var result = await _handler.Handle(new DeleteRoomCommand(1, 999), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _rooms.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ThrowsAsync(new Exception("unexpected"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(new DeleteRoomCommand(1, 101), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesCorrectHotelIdAndNumberToRepository()
        {
            _rooms.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Success("deleted"));

            await _handler.Handle(new DeleteRoomCommand(hotelId: 5, number: 302), CancellationToken.None);

            _rooms.Verify(r => r.Delete(5, 302, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
