using Core.Enums;

namespace Api.Contracts
{
    public record UpdateRoomForm
    (
        string Name,
        string Description,
        decimal Price,
        int Rating,
        bool Pool,
        RoomCategory RoomCategory,
        List<IFormFile>? Images
    );    
}
