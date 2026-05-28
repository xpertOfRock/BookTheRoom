using Application.Interfaces;
using Application.UseCases.Commands.Apartment;
using Application.UseCases.Handlers.CommandHandlers.Apartment;
using Core.Interfaces;
using Core.TasksResults;

namespace Tests.ApplicationTests.Handlers
{
    public class DeleteApartmentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IApartmentRepository> _apartments = new();
        private readonly DeleteApartmentCommandHandler _handler;

        public DeleteApartmentCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Apartments).Returns(_apartments.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new DeleteApartmentCommandHandler(_unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WhenDeletionSucceeds_CommitsTransaction()
        {
            _apartments
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("Entity 'Apartment' was deleted successfully."));

            var command = new DeleteApartmentCommand(id: 1, userId: "owner-1");

            IResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenOwnerMismatch_RollsBackAndReturnsFail()
        {
            _apartments
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fail("Mismatch between OwnerId property and passed value."));

            var command = new DeleteApartmentCommand(id: 1, userId: "intruder");

            IResult result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PassesCommandArgumentsToRepository()
        {
            _apartments
                .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("deleted"));

            var command = new DeleteApartmentCommand(id: 42, userId: "owner-42");

            await _handler.Handle(command, CancellationToken.None);

            // Проверяем, что хендлер прокидывает корректные аргументы в репозиторий.
            _apartments.Verify(
                r => r.Delete(42, "owner-42", It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
