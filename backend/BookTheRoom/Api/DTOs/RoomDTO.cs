using Core.Enums;

namespace Api.DTOs
{
    public record RoomDTO
    (
        int HotelId,
        string Name,
        string Description,
        int Number,
        decimal Price,
        List<string>? Images,
        RoomCategory Category
    );
}
