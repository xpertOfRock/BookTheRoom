namespace Application.UseCases.Queries.Chat
{
    public class GetChatByIdQuery : IQuery<Core.Entities.Chat>
    {
        public Guid Id { get; }
        public GetChatByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
