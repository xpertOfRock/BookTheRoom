using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Handlers.CommandHandlers.Hotel;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Core.TasksResults;
using Core.ValueObjects;

namespace Tests.ApplicationTests.Handlers
{
    public class CreateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHotelRepository> _hotels = new();
        private readonly CreateHotelCommandHandler _handler;

        public CreateHotelCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Hotels).Returns(_hotels.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new CreateHotelCommandHandler(_unitOfWork.Object);
        }

        private static CreateHotelCommand BuildCommand()
        {
            var request = new CreateHotelRequest(
                Name: "Grand Hotel",
                Description: "A pleasant place to stay.",
                Rating: 4,
                Pool: true,
                Address: new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000"),
                Images: new List<string> { "https://example.com/image1.jpg" });

            return new CreateHotelCommand(request);
        }

        [Fact]
        public async Task Handle_WhenRepositorySucceeds_CommitsAndReturnsSuccess()
        {
            _hotels
                .Setup(r => r.Add(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("Entity 'Hotel' was created successfully."));

            IResult result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _hotels.Verify(r => r.Add(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_RollsBackAndDoesNotCommit()
        {
            _hotels
                .Setup(r => r.Add(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fail("Entity with this address and name already exists."));

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
                .Setup(r => r.Add(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("database is down"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(BuildCommand(), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
