using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Handlers.CommandHandlers.Hotel;
using Core.Interfaces;
using Core.TasksResults;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for DeleteHotelCommandHandler.
    // Verifies the transaction branching logic: commit on success, rollback on failure,
    // and exception wrapping — all without touching a real database.
    public class DeleteHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHotelRepository> _hotels = new();
        private readonly DeleteHotelCommandHandler _handler;

        public DeleteHotelCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Hotels).Returns(_hotels.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new DeleteHotelCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WhenDeletionSucceeds_CommitsTransaction()
        {
            _hotels
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("Entity 'Hotel' was deleted successfully."));

            var command = new DeleteHotelCommand(id: 1);

            IResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenHotelNotFound_RollsBackAndReturnsFail()
        {
            _hotels
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fail("Hotel with this ID doesn't exist."));

            var command = new DeleteHotelCommand(id: 99);

            IResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _hotels
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("database is down"));

            var command = new DeleteHotelCommand(id: 1);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesCorrectIdToRepository()
        {
            _hotels
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("deleted"));

            var command = new DeleteHotelCommand(id: 42);

            await _handler.Handle(command, CancellationToken.None);

            _hotels.Verify(r => r.Delete(42, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
