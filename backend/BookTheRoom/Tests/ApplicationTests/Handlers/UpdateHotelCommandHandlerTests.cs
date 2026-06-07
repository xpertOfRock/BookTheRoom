using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Handlers.CommandHandlers.Hotel;
using Core.Contracts;
using Core.Interfaces;
using Core.TasksResults;
using Core.ValueObjects;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for UpdateHotelCommandHandler.
    // Verifies the commit/rollback branching logic using Moq doubles,
    // keeping the tests independent of the real infrastructure.
    public class UpdateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHotelRepository> _hotels = new();
        private readonly UpdateHotelCommandHandler _handler;

        public UpdateHotelCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Hotels).Returns(_hotels.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new UpdateHotelCommandHandler(_unitOfWork.Object);
        }

        private static UpdateHotelCommand BuildCommand(int id = 1)
        {
            var request = new UpdateHotelRequest(
                Name: "Grand Hotel Updated",
                Description: "Updated description.",
                Rating: 4,
                HasPool: true,
                Address: new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000"),
                Images: new List<string> { "https://example.com/img1.jpg" });

            return new UpdateHotelCommand(id, request);
        }

        [Fact]
        public async Task Handle_WhenUpdateSucceeds_CommitsAndReturnsSuccess()
        {
            _hotels
                .Setup(r => r.Update(It.IsAny<int>(), It.IsAny<UpdateHotelRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("Entity 'Hotel' was updated successfully."));

            IResult result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenHotelNotFound_RollsBackAndDoesNotCommit()
        {
            _hotels
                .Setup(r => r.Update(It.IsAny<int>(), It.IsAny<UpdateHotelRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fail("Hotel with this ID doesn't exist."));

            IResult result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _hotels
                .Setup(r => r.Update(It.IsAny<int>(), It.IsAny<UpdateHotelRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("database timeout"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(BuildCommand(), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesCorrectIdToRepository()
        {
            _hotels
                .Setup(r => r.Update(It.IsAny<int>(), It.IsAny<UpdateHotelRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("updated"));

            await _handler.Handle(BuildCommand(id: 77), CancellationToken.None);

            _hotels.Verify(
                r => r.Update(77, It.IsAny<UpdateHotelRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
