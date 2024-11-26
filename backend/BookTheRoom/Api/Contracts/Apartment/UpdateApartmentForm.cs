namespace Api.Contracts.Apartment
{
    public record UpdateApartmentForm
    (
        string Title,
        string Description,
        decimal PricePerNight,
        List<IFormFile>? Images
    );
}
