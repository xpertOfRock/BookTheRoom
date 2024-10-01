namespace Api.Contracts
{
    public record UpdateApartmentForm
    (
        string Title,
        string Description,
        decimal PricePerNight,
        List<IFormFile>? Images
    );
}
