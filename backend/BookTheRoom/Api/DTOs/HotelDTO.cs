using Core.Entities;
using Core.ValueObjects;

namespace Api.DTOs
{
    public record HotelDTO(
        int Id,
        string Name,
        string Description,
        List<string>? Images,
        List<Room>? Rooms,
        Address Address
        );
}
