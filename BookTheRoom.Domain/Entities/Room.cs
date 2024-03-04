using BookTheRoom.Domain.Enums;

namespace BookTheRoom.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal PriceForRoom { get; set; }          
        public bool IsFree { get; set; }
        public string Image { get; set; }
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }
        public RoomCategory RoomCategory { get; set; }
    }
}
