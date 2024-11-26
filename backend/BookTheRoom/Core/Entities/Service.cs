using Core.Enums;

namespace Core.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public HotelService ServiceName { get; set; }
        public decimal Price { get; set; }
    }
}
