using Core.Enums;

namespace Core.Contracts
{
    public record CreateRoomRequest(
        string Name,
        string Description,
        int Number,
        decimal Price,
        RoomCategory Category
        );
}
