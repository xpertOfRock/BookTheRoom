using Core.Enums;

namespace Api.DTOs
{
    public record RoomsDTO
    (
        int HotelId,
        string Name,
        int Number,
        string Preview,
        RoomCategory Category
    );
}
