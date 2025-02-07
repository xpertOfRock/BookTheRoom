namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public float? UserScore { get; set; }
        public int? HotelId { get; set; } = null;
        public int? ApartmentId { get; set; } = null;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; } = null;
    }
}
