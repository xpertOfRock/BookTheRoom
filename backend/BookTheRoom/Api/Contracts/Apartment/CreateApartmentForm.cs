namespace Api.Contracts.Apartment
{
    public record CreateApartmentForm
    (
        string Title,
        string Description,
        decimal PriceForNight,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        List<IFormFile> Images,
        string? Telegram,
        string? Instagram
    );
}
