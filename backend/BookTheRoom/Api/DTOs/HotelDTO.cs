using Core.Entities;
using Core.ValueObjects;

namespace Api.DTOs
{
    public record HotelDTO
    (
        int Id,
        string Name,
        string Description,
        string Address,
        List<string>? Images,
        List<Room>? Rooms,
        List<Comment>? Comments
    );
}
