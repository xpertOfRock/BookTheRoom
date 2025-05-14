using Core.Abstractions;

namespace Core.Entities
{
    public class Chat : IEntity
    {
        public Guid Id { get; set; }
        public int? ApartmentId { get; set; } = null; // (Just explaination, dont use that yet) Chat system will be used in Apartment entity (in case you want to know more about renting one's owners apartment you can communicate with him through online chat)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<string> UsersId { get; set; } // I'm using ASP.NET Identity, but ApplicationUser located in Infrastructure layer
        public List<ChatMessage> Messages { get; set; }
    }
}
