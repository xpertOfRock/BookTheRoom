using Core.ValueObjects;

namespace Core.Contracts
{
    public record CreateHotelRequest
    (
        string Name,
        string Description,
        int Rating,
        bool Pool,
        Address Address, 
        List<string> Images
    );
}