using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.WebUI.ViewModels
{
    public class AddRoomViewModel : Room
    {
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> RoomImages { get; set; }
    }
}
