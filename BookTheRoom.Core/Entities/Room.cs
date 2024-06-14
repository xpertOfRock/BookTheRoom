using BookTheRoom.Domain.Enums;

namespace BookTheRoom.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public decimal PriceForRoom { get; set; }
        public bool IsFree { get; set; } = true; 
        public int HotelId { get; set; }
        public string PreviewURL { get; set; } = string.Empty;

        public Hotel Hotel { get; set; }
        public RoomCategory RoomCategory { get; set; }
        public List<string>? ImagesURL { get; set; }
    }
}
