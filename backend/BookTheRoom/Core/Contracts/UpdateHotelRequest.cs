namespace Core.Contracts
{
    public record UpdateHotelRequest
    (
        string Name,
        string Description,
        uint Rating,
        bool HasPool,
        List<string>? Images
    );
}
