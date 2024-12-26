using Core.ValueObjects;

namespace Core.Contracts
{
    public record UpdateHotelRequest
    (
        string Name,
        string Description,
        int Rating,
        bool HasPool,
        Address Address,
        List<string>? Images
    );
}
