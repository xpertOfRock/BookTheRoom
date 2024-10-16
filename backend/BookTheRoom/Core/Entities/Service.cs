using Core.Enums;

namespace Core.Entities
{
    public class Service
    {
        public int HotelId { get; set; }

        public OrderOptionsName OptionName { get; set; }
        public decimal OptionPrice { get; set; }
    }
}
