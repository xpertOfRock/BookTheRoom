namespace Application.UseCases.Commands.Comment
{
    public class CreateCommentCommand : ICommand<IResult>
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int? HotelId { get; set; }
        public int? ApartmentId { get; set; }
        public string Description { get; set; }
        public float? UserScore { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateCommentCommand
        (
            string userId,
            string username,
            string description,
            float? userScore,
            int? hotelId = null,
            int? apartmentId = null            
        )
        {
            UserId = userId;
            Username = username;
            HotelId = hotelId;
            ApartmentId = apartmentId;
            Description = description;
            UserScore = userScore ?? null;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }
    }
}
