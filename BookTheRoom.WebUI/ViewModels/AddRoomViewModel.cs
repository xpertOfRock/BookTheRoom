using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.WebUI.ViewModels
{
    public class AddRoomViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public decimal PriceForRoom { get; set; }
        public bool IsFree { get; set; }
        public string PreviewURL { get; set; }

        public Hotel Hotel { get; set; }
        public RoomCategory RoomCategory { get; set; }
        public List<string>? ImagesURL { get; set; }
    }
}
