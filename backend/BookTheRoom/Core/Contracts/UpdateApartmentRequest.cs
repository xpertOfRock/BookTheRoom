using Core.Entities;
using Core.ValueObjects;

namespace Core.Contracts
{
    public record UpdateApartmentRequest
    (
        string Title,
        string Description,
        decimal Price,
        Address Address,
        List<string>? Images
    );
}
