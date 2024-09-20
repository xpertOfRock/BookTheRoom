namespace Api.Contracts
{
    public record CreateRoomForm
    (        
        string Title,
        string Description,
        decimal PricePerNight,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        List<IFormFile> Images
    );
    
}
