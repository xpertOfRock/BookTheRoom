using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Room
{
    public class CreateRoomCommand : IRequest<Unit>
    {
        public Core.Entities.Room Room { get; set; }
        public CreateRoomCommand(Core.Entities.Room room)
        {
            Room = room;
        }
    }
}
