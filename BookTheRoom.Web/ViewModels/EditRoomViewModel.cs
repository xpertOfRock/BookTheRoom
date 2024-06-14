using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.WebUI.ViewModels
{
    public class EditRoomViewModel : Room
    {
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile>? RoomImages { get; set; }
    }
}
