using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Room
{
    public class UpdateRoomCommand : IRequest<Unit>
    {
        public Core.Entities.Room Room { get; set; }
        public UpdateRoomCommand(Core.Entities.Room room)
        {
            Room = room;
        }
    }
}
