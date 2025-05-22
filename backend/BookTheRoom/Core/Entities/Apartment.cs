using Core.Abstractions;
using Core.ValueObjects;

namespace Core.Entities
{
    public class Apartment : IEntity
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PriceForNight { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<string> Images { get; set; }
        public List<Chat>? Chats { get; set; }
        public List<Comment>? Comments { get; set; }
        public Address Address { get; set; }
    }
}
