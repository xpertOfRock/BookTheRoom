namespace Api.DTOs
{    
    public record ApartmentDTO
    (
        int Id,
        string Title,
        string Description,
        string Owner,
        string Email,
        string PhoneNumber,
        string Telegram,
        string Instagram,
        string Address,
        DateTime CreatedAt,
        List<string>? Images,
        List<Comment>? Comments,
        List<Core.Entities.Chat> Chats
    );
}
