using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BookTheRoom.WebUI.Controllers
{
    public class HotelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IPhotoService _photoService;    , IPhotoService photoService
        public HotelController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            //_photoService = photoService;

        }
        // GET: HotelController
        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Hotel> hotels =  await _unitOfWork.Hotels.GetAllInclude();
            return View(hotels);
        }

        // GET: HotelController/Details/5
        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Detail(int id)
        {
            Hotel hotel = await _unitOfWork.Hotels.GetByIdGetByIdInclude(id);
            if (hotel == null)
            {
                return Redirect("/Home/Index");

            }
            return View(hotel);
        }

        // GET: HotelController/Create
        public IActionResult CreateHotel()
        {
            return View();
        }

        // POST: HotelController/Create
        [HttpPost]
        public IActionResult CreateHotel(CreateHotelViewModel collection)
        {
            if (!ModelState.IsValid)
            {

            }
            return View(collection);
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
        // GET: HotelController/Edit/5
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
