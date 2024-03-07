using BookTheRoom.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        // GET: HotelController
        public async Task<IActionResult> Index()
        {
            var hotels =  await _unitOfWork.Hotels.GetAll();
            return View(hotels);
        }

        // GET: HotelController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetById(id);
            return View(hotel);
        }

        // GET: HotelController/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: HotelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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
