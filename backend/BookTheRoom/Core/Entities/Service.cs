using Core.Abstractions;
using Core.Enums;

namespace Core.Entities
{
    public class Service : IEntity
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public HotelService ServiceName { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
