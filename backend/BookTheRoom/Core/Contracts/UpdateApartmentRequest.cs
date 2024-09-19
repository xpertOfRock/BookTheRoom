using Core.Entities;

namespace Core.Contracts
{
    public record UpdateApartmentRequest
    (
        string Title,
        string Description,
        decimal Price,
        List<string> Images,
        List<Comment> Comments
    );
}
