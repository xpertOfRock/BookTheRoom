using Core.Entities;
using Core.Enums;

namespace Core.Contracts
{
    public record UpdateRoomRequest
    (
        string Name,
        string Description,
        decimal Price,
        bool IsFree,
        RoomCategory Category,
        List<string> Images
    );
}
