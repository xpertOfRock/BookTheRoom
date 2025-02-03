using Core.ValueObjects;

namespace Core.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PriceForNight { get; set; }

        public List<string> Images { get; set; }
        public List<Comment>? Comments { get; set; }
        public Address Address { get; set; }
    }
}
