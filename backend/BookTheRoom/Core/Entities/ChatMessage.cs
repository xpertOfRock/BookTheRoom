using Core.Abstractions;

namespace Core.Entities
{
    public class ChatMessage : IEntity
    {
        public int Id { get; set; }
        public Guid ChatId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}