using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Room
{
    public class DeleteRoomCommand : IRequest<Unit>
    {
        public Core.Entities.Room Room { get; set; }
        public DeleteRoomCommand(Core.Entities.Room room)
        {
            Room = room;
        }
    }
}
