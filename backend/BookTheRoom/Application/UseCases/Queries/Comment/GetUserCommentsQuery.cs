namespace Application.UseCases.Queries.Comment
{
    public class GetUserCommentsQuery : IQuery<List<Core.Entities.Comment>>
    {
        public string UserId { get; }
        public GetUserCommentsRequest Filter { get; }
        public GetUserCommentsQuery(string userId, GetUserCommentsRequest filter)
        {
            UserId = userId;
            Filter = filter;
        }
    }
}
