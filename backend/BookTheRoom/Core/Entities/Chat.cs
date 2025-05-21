using Core.Abstractions;

namespace Core.Entities
{
    public class Chat : IEntity
    {
        public Guid Id { get; set; } 
        public int? ApartmentId { get; set; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public List<string> UsersId { get; set; }
        public List<ChatMessage>? Messages { get; set; }
    }
}
