using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.Enums;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Infrastructure.Data.Interfaces;
using BookTheRoom.Web.ViewModels;
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookTheRoom.Web.Extencions;

namespace BookTheRoom.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPaymentService _braintreeService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork,
                               IEmailService emailService, IPaymentService braintreeService,
                               UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _contextAccessor = contextAccessor;
            _braintreeService = braintreeService;
            _unitOfWork = unitOfWork;
        }

        [Route("/Profile/{userName}/Orders", Name = "UserOrders")]
        public async Task<IActionResult> UserOrders([FromRoute] string userName)
        {
            
            var thisUser = await _userManager.FindByNameAsync(userName);
            var userId = await _userManager.GetUserIdAsync(thisUser!);
            var userOrders = await _unitOfWork.Orders.GetUserOrders(userId);            
            var thisOrders = new List<OrderViewModel>();

            foreach (var order in userOrders)
            {
                var thisHotel = await _unitOfWork.Hotels.GetById(order.HotelId);
                var thisRoom = await _unitOfWork.Rooms.GetById(order.RoomId);
                thisOrders.Add(new OrderViewModel
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Status = order.Status,
                    AnonymousEmail = order.AnonymousEmail,
                    AnonymousNumber = order.AnonymousNumber,
                    CheckIn = order.CheckIn,
                    CheckOut = order.CheckOut,
                    OverallPrice = Math.Round(order.OverallPrice, 2),
                    Hotel = thisHotel,
                    HotelId = order.HotelId,
                    Room = thisRoom,
                    RoomId = order.RoomId
                });
            }
            return View(thisOrders);
        }

        [Authorize(Roles = UserRole.Admin)]
        [Route("Orders", Name = "Orders")]
        public async Task<IActionResult> Orders() {
                   
            var orders = await _unitOfWork.Orders.GetAll();
            var thisOrders = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var thisHotel = await _unitOfWork.Hotels.GetById(order.HotelId);
                var thisRoom = await _unitOfWork.Rooms.GetById(order.RoomId);
                thisOrders.Add(new OrderViewModel
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Status = order.Status,
                    AnonymousEmail = order.AnonymousEmail,
                    AnonymousNumber = order.AnonymousNumber,
                    CheckIn = order.CheckIn,
                    CheckOut = order.CheckOut,
                    OverallPrice = Math.Round(order.OverallPrice, 2),
                    Hotel = thisHotel,
                    HotelId = order.HotelId,
                    Room = thisRoom,
                    RoomId = order.RoomId
                });
            }            
            return View(thisOrders);
        }

        
        [Authorize(Roles = UserRole.User)]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Profile/Orders/{id}", Name = "Order")]
        public async Task<IActionResult> Order(int id)
        {           
            var order = await _unitOfWork.Orders.GetById(id);
            var thisHotel = await _unitOfWork.Hotels.GetById(order.HotelId);
            var thisRoom = await _unitOfWork.Rooms.GetById(order.RoomId);

            var showOrder = new OrderViewModel
            {
                Id = id,
                UserId = order.UserId,
                Status = order.Status,
                AnonymousEmail = order.AnonymousEmail,
                AnonymousNumber = order.AnonymousNumber,
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,             
                OverallPrice = Math.Round(order.OverallPrice, 2),
                Hotel = thisHotel,
                HotelId = order.HotelId,
                Room = thisRoom,                
                RoomId = order.RoomId
            };
            return View(showOrder);
        }


        [Route("/Checkout", Name = "Checkout")]
        public async Task<IActionResult> Checkout(int hotelId, int roomId)
        {
            string? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = _contextAccessor.HttpContext?.User.GetUserId();
            }
            var gateway = _braintreeService.CreateGateway();
            var clientToken = gateway.ClientToken.Generate();
            
            var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
            var thisRoom = await _unitOfWork.Rooms.GetById(roomId);

            ViewBag.ClientToken = clientToken;

            var orderViewModel = new OrderViewModel
            {
                HotelId = hotelId,
                UserId = currentUserId,
                RoomId = roomId,
                Hotel = thisHotel,
                Room = thisRoom
            };
            return View(orderViewModel);
        }

        //  4217651111111119
        [HttpPost]
        [Route("/Checkout", Name = "Checkout")]
        public async Task<IActionResult> Checkout(OrderViewModel order)
        {
            var thisHotel = await _unitOfWork.Hotels.GetById(order.HotelId);
            var thisRoom = await _unitOfWork.Rooms.GetById(order.RoomId);

            var gateway = _braintreeService.CreateGateway();

            TimeSpan duration = order.CheckOut.Subtract(order.CheckIn);
            int days = (int)Math.Ceiling(duration.TotalDays);

            order.Status = OrderStatus.Active;
            order.OverallPrice = thisRoom.PriceForRoom * days;
                     
            string nonceFromClient = Request.Form["payment_method_nonce"]!;

            var request = new TransactionRequest
            {
                Amount = order.OverallPrice,
                OrderId = order.Id.ToString(),
                PaymentMethodNonce = nonceFromClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = await gateway.Transaction.SaleAsync(request);
            if (result.Target.ProcessorResponseText == "Approved")
            {
                TempData["Success"] = "Transaction was successful.\n" +
                                      $"Transaction ID: {result.Target.Id}\n" +
                                      $"Amount: {result.Target.Amount}";
            }
          
            var thisUser = await _userManager.FindByIdAsync(order.UserId!);
         
            await _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync();

            string email;
            string subject = $"Order No. {order.Id}";
            string body = "Thanks for choosing Book The Room!\n\n" +
                         $"Your hotel: {thisHotel.Name}\n" +
                         $"Address: {thisHotel.Address.Country}, {thisHotel.Address.City}, {thisHotel.Address.StreetOrDistrict}, {thisHotel.Address.Index}\n" +
                         $"Room No. : {thisRoom.Number}\n" +
                         $"Duration: {days} days\n" +
                         $"Check in date: {order.CheckIn.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Check out date: {order.CheckOut.ToString("dd.MM.yyyy HH:mm")}\n" +
                         $"Overall price : {Math.Round(order.OverallPrice, 2)}\n\n" +
                         $"Have a nice day!";

            if (order.UserId == null)
            {
                email = order.AnonymousEmail;
            }
            else
            {
                email = thisUser.Email;
            }
            _emailService.SendEmail(email, subject, body);
            return RedirectToAction("Index", "Home");
        }
    }
}

