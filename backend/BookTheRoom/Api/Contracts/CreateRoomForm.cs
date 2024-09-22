namespace Api.Contracts
{
    public record CreateRoomForm
    (   
        int number,
        string Title,
        string Description,
        decimal PricePerNight,      
        List<IFormFile> Images
    );
    
}
