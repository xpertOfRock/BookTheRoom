namespace Api.Contracts.Chat
{
    public record CreateChatRequest
    (
        List<string> UserIds,
        int? ApartmentId = null
    );
}
