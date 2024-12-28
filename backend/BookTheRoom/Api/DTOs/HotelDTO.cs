using Core.Entities;

namespace Api.DTOs
{
    public record HotelDTO
    (
        int Id,
        string Name,
        string Description,
        string Address,
        string jsonAddress,
        bool HasPool,
        int Rating, 
        List<string>? Images,
        List<Comment>? Comments
    );
}