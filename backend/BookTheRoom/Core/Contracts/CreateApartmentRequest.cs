namespace Core.Contracts
{
    public record CreateApartmentRequest
    (
        string Title,
        string Description,
        decimal Price,
        List<string> Images
    );
}
