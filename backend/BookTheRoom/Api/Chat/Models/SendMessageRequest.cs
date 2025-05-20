namespace Api.Chat.Models
{
    public record SendMessageRequest
    (
        string ChatId,
        string Message        
    );
}
