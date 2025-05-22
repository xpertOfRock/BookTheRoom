using Core.ValueObjects;

namespace Core.Contracts
{
    public record CreateApartmentRequest
    (
        string Title,
        string Description,
        decimal Price,
        Address Address,
        List<string> Images,
        string? Telegram,
        string? Instagram
    );
}