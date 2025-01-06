namespace Api.Contracts.Room
{
    public record CreateRoomForm
    (
        string Title,
        string Description,
        int Number,
        decimal PricePerNight,
        int Category,
        List<IFormFile> Images
    );
}
