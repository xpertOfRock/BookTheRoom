using Core.ValueObjects;

namespace Core.Contracts
{
    public record CreateApartmentRequest
    (
        string Title,
        string Description,
        string OwnerId,
        string OwnerName,
        decimal Price,
        Address Address,
        List<string> Images
    );
}
