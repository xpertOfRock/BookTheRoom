using Core.Entities;

namespace Api.DTOs
{
    public record HotelDTO
    (
        int Id,
        string Name,
        string Description,
        string Address,
        int Rating, 
        List<string>? Images,
        List<Room>? Rooms,
        List<Comment>? Comments
    );
}