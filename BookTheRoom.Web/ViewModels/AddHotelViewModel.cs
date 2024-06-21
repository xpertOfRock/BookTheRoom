using BookTheRoom.Core.Entities;

namespace BookTheRoom.Web.ViewModels
{
    public class AddHotelViewModel : Hotel
    {        
        public IFormFile PreviewImage { get; set; }
        public List<IFormFile> HotelImages { get; set; }

    }
}
