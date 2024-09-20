namespace Api.Contracts
{
    public record UpdateApartmentForm
    (
        string Title,
        string Description,
        int Rating,
        bool Pool,
        List<IFormFile>? Images
    );
}
