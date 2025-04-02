namespace Api.Contracts.Comment
{
    public record CreateCommentForm
    (
        int PropertyId,
        Core.Enums.PropertyType PropertyType,
        string Description,
        float? UserScore
    );
    
}
