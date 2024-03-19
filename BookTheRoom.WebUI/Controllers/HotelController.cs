using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookTheRoom.WebUI.Controllers
{
    public class HotelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;
        public HotelController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;

        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Hotels()
        {
            IEnumerable<Hotel> hotels = await _unitOfWork.Hotels.GetAllInclude();
            return View(hotels);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Hotel(int id)
        {
            Hotel hotel = await _unitOfWork.Hotels.GetByIdGetByIdInclude(id);
            if (hotel == null)
            {
                return Redirect("/Hotel/Index");

            }
            return View(hotel);
        }

        [HttpGet]
        [Route("Hotels/Hotel/{hotelId:int}/Rooms", Name = "Rooms")]
        public async Task<IActionResult> Rooms(int hotelId)
        {
            IEnumerable<Room> rooms = await _unitOfWork.Rooms.GetAllRoomsByHotel(hotelId);
            return View(rooms);
        }

        [HttpGet]
        [Route("Hotels/Hotel/{hotelId:int}/Rooms/Room/{roomId:int}", Name = "Room")]
        public async Task<IActionResult> Room(int hotelId, int roomId)
        {
            Room room = await _unitOfWork.Rooms.GetRoomByIdAsync(hotelId, roomId);
            if (room == null)
            {
                return Redirect("/Hotel/Index");

            }
            return View(room);
        }

        public IActionResult AddHotel()
        {
            return View();
        }

        [HttpPost]
        [Route("Hotels/Add", Name = "AddHotel")]
        public async Task<IActionResult> AddHotel(AddHotelViewModel addHotelViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var result = await _photoService.AddPhotoAsync(addHotelViewModel.PreviewImage.Name,

                                                               addHotelViewModel.PreviewImage.OpenReadStream());

                List<string> hotelImages = new List<string>();

                foreach (var item in addHotelViewModel.HotelImages)
                {
                    var resultForList = await _photoService.AddPhotoAsync(item.Name, item.OpenReadStream());
                    hotelImages.Add(resultForList.Url.ToString());
                }
                

                var hotel = new Hotel
                {
                    Name = addHotelViewModel.Name,
                    Description = addHotelViewModel.Description,
                    NumberOfRooms = addHotelViewModel.NumberOfRooms,
                    HasPool = addHotelViewModel.HasPool,
                    Rating = addHotelViewModel.Rating,
                    PreviewURL = result.Url.ToString(),
                    ImagesURL = hotelImages,
                    Address = new Address
                    {
                        Country = addHotelViewModel.Address.Country,
                        City = addHotelViewModel.Address.City,
                        StreetOrDistrict = addHotelViewModel.Address.StreetOrDistrict,
                        Index = addHotelViewModel.Address.Index
                    },
                   
                };

                await _unitOfWork.Hotels.Add(hotel);

                _unitOfWork.Complete();

                return RedirectToAction("Hotels");
            }
            return View(addHotelViewModel);
        }
        


        public IActionResult AddRoom()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddRoom(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HotelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HotelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HotelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
