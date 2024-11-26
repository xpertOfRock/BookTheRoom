using Core.Enums;

namespace Api.Contracts.Room
{
    public record CreateRoomForm
    (
        string Title,
        string Description,
        int Number,
        decimal PricePerNight,
        RoomCategory Category,
        List<IFormFile> Images
    );

}
