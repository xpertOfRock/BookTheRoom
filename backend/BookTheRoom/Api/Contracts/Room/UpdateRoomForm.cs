using Core.Enums;

namespace Api.Contracts.Room
{
    public record UpdateRoomForm
    (
        string Name,
        string Description,
        decimal Price,
        RoomCategory Category,
        List<IFormFile>? Images
    );
}
