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
            var hotel = await _unitOfWork.Hotels.GetById(command.HotelId);
            

            await _unitOfWork.Rooms.Add(new Core.Entities.Room 
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

            hotel.Rooms!.Add(room);

            await _unitOfWork.Hotels.Update(
                command.HotelId,
                new UpdateHotelRequest(
                    hotel.Name,
                    hotel.Description,
                    hotel.Rating,
                    hotel.RoomsAmount,
                    hotel.HasPool,
                    hotel.Images,
                    hotel.Rooms
                    )
                );

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
