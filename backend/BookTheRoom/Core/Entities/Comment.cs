namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public int HotelId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
