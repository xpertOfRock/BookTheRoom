using Core.Enums;

namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public int? HotelId { get; set; } = null;
        public int? ApartmentId { get; set; } = null;
        public PropertyCategory PropetyCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
