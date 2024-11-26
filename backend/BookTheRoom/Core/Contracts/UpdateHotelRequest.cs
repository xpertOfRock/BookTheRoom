using Core.Entities;

namespace Core.Contracts
{
    public record UpdateHotelRequest
    (
        string Name,
        string Description,
        int Rating,
        bool HasPool,
        List<string>? Images
    );
}
