using Core.Entities;
using Core.Enums;

namespace Core.Entities
{
    public class Room
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsFree { get; set; } = true;

        public List<string>? Images { get; set; }
        public RoomCategory Category { get; set; }           
    }
}
