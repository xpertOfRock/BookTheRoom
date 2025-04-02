namespace Application.UseCases.Commands.Comment
{
    public class CreateCommentCommand : ICommand<IResult>
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int PropertyId { get; set; }
        public PropertyType PropertyType { get; set; }
        public string Description { get; set; }
        public float? UserScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateCommentCommand
        (
            string userId,
            string username,
            string description,
            int ropertyId,
            PropertyType propertyType,
            float? userScore          
        )
        {
            UserId = userId;
            Username = username;
            PropertyId = ropertyId;
            PropertyType = propertyType;
            Description = description;
            UserScore = userScore ?? null;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }
    }
}
