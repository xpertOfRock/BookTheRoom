namespace Api.Contracts
{
    public record UpdateRoomForm
    (
        string Name,
        string Description,
        int Rating,
        bool Pool,
        List<IFormFile>? Images
    );
    
}
