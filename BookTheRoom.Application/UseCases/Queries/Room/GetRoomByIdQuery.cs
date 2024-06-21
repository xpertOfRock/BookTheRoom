using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Room
{
    public class GetRoomByIdQuery : IRequest<Core.Entities.Room>
    {
        public int RoomId { get; set; }
        public GetRoomByIdQuery(int roomId)
        {
            RoomId = roomId;
        }
    }
}
