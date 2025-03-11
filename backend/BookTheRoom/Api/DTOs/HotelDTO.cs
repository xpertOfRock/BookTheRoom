using Core.Entities;

namespace Api.DTOs
{
    public record HotelDTO
    (
        int Id,
        string Name,
        string Description,
        string Address,
        string JsonAddress,
        bool HasPool,
        int Rating,      
        float? UserScore,
        List<string>? Images,
        List<Comment>? Comments
    );
}