namespace Application.UseCases.Queries.Chat
{
    public class GetChatByUserIdQuery : IQuery<Core.Entities.Chat?>
    {
        public string UserId { get; }
        public int ApartmentId { get; set; }
        public GetChatByUserIdQuery(string userId, int apartmentId)
        {
            UserId = userId;
            ApartmentId = apartmentId;
        }
    }
}
