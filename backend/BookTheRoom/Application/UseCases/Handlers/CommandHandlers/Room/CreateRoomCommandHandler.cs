using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Core.Contracts;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var hotel = await _unitOfWork.Hotels.GetById(command.HotelId);

                await _unitOfWork.Rooms.Add
                (
                    new Core.Entities.Room
                    {
                        Name = command.Name,
                        Description = command.Description,
                        Number = command.Number,
                        Price = command.Price,
                        Category = command.Category,
                        HotelId = command.HotelId,
                    }
                );

                var room = await _unitOfWork.Rooms.GetById(command.HotelId, command.Number);

                var images = command.Images.Any() ? command.Images : hotel.Images;

                hotel.Rooms!.Add(room);

                await _unitOfWork.Hotels.Update
                    (command.HotelId,
                        new UpdateHotelRequest
                        (
                            hotel.Name,
                            hotel.Description,
                            hotel.Rating,
                            hotel.HasPool,
                            images
                        )
                    );

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
            return Unit.Value;
        }
    }
}
