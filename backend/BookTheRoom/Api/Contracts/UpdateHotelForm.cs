namespace Api.Contracts
{
    public record UpdateHotelForm
    (
        string Name,
        string Description,
        int Rating,
        bool Pool,
        List<IFormFile>? Images
    );
}
