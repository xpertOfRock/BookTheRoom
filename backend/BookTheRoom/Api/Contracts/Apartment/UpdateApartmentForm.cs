namespace Api.Contracts.Apartment
{
    public record UpdateApartmentForm
    (
        string Title,
        string Description,
        decimal Price,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        List<IFormFile>? Images
    );
}
