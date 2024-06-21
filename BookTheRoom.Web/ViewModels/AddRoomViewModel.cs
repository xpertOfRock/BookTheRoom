using BookTheRoom.Core.Entities;

namespace BookTheRoom.Web.ViewModels
{
    public class AddRoomViewModel : Room
    {
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> RoomImages { get; set; }
    }
}
