using Application.Interfaces;
using Application.UseCases.Commands.Apartment;
using Application.UseCases.Handlers.CommandHandlers.Apartment;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Core.TasksResults;
using Core.ValueObjects;

namespace Tests.ApplicationTests.Handlers
{
    // Unit tests for CreateApartmentCommandHandler.
    // Covers the transaction lifecycle and entity mapping, using Moq doubles
    // instead of a real database or infrastructure layer.
    public class CreateApartmentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IApartmentRepository> _apartments = new();
        private readonly CreateApartmentCommandHandler _handler;

        public CreateApartmentCommandHandlerTests()
        {
            _unitOfWork.Setup(u => u.Apartments).Returns(_apartments.Object);
            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                       .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

            _handler = new CreateApartmentCommandHandler(_unitOfWork.Object);
        }

        private static CreateApartmentCommand BuildCommand(
            string ownerId = "owner-1",
            string ownerName = "John Doe",
            string email = "john@example.com",
            string phone = "+380000000000")
        {
            var request = new CreateApartmentRequest(
                Title: "Cozy Studio",
                Description: "A cozy studio in city center.",
                Price: 150m,
                Address: new Address("Ukraine", "Lviv Oblast", "Lviv", "Svobody Ave 1", "79000"),
                Images: new List<string> { "https://example.com/img1.jpg" },
                Telegram: "@owner",
                Instagram: null);

            return new CreateApartmentCommand(ownerId, ownerName, email, phone, request);
        }

        [Fact]
        public async Task Handle_WhenRepositorySucceeds_CommitsAndReturnsSuccess()
        {
            _apartments
                .Setup(r => r.Add(It.IsAny<Apartment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Success("Entity 'Apartment' was created successfully."));

            IResult result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.True(result.IsSuccess);
            _apartments.Verify(r => r.Add(It.IsAny<Apartment>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_RollsBackAndDoesNotCommit()
        {
            _apartments
                .Setup(r => r.Add(It.IsAny<Apartment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Fail("Apartment with this address already exists."));

            IResult result = await _handler.Handle(BuildCommand(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_WrapsExceptionAndRollsBack()
        {
            _apartments
                .Setup(r => r.Add(It.IsAny<Apartment>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("connection lost"));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(BuildCommand(), CancellationToken.None));

            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_MapsCommandFieldsToApartmentEntity()
        {
            Apartment? capturedApartment = null;

            _apartments
                .Setup(r => r.Add(It.IsAny<Apartment>(), It.IsAny<CancellationToken>()))
                .Callback<Apartment, CancellationToken>((apt, _) => capturedApartment = apt)
                .ReturnsAsync(new Success("created"));

            var command = BuildCommand(ownerId: "u-99", ownerName: "Jane Doe",
                                       email: "jane@example.com", phone: "+380111111111");

            await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(capturedApartment);
            Assert.Equal("u-99", capturedApartment!.OwnerId);
            Assert.Equal("Jane Doe", capturedApartment.OwnerName);
            Assert.Equal("jane@example.com", capturedApartment.Email);
            Assert.Equal("+380111111111", capturedApartment.PhoneNumber);
        }
    }
}
