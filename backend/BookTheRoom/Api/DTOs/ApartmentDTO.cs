namespace Api.DTOs
{    
    public record ApartmentDTO
    (
        int Id,
        string Title,
        string Description,
        string OwnerId,
        string Owner,
        string Email,
        string PhoneNumber,
        decimal Price,
        string Telegram,
        string Instagram,
        string JsonAddress,
        string Address,
        float? UserScore,
        DateTime CreatedAt,
        List<string>? Images,
        List<Comment>? Comments,
        List<Core.Entities.Chat> Chats
    );
}
