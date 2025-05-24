namespace Api.Contracts.Comment
{
    public record GetUserCommentsResponse(List<Core.Entities.Comment> Comments);
}
