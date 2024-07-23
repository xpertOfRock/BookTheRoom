using Api.Extensions;
using Application.Interfaces;
using Application.UseCases.Commands.Order;
using Braintree;
using Core.Contracts;
using Core.Entities;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(
            IMediator mediator,
            IEmailService emailService,
            IPaymentService paymentService,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _mediator = mediator;
            _emailService = emailService;
            _paymentService = paymentService;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

            //[HttpGet]
            //public async Task<IActionResult> GetAll()
            //{
            //    return new string[] { "value1", "value2" };
            //}

            //[HttpGet("{id}")]
            //public async Task<IActionResult> GetAllUsers(int userId)
            //{
            //    return "value";
            //}

        [HttpPost]
        public async Task<IActionResult> Post(int hotelId, int number, [FromBody] CreateOrderRequest request)
        {
            string? userId = null;

            if(User.Identity.IsAuthenticated)
            {
                userId = _contextAccessor.HttpContext?.User.GetUserId();
            }

            var nonceFromClient = Request.Form["payment_method_nonce"]!;
                     
            await _mediator.Send(new CreateOrderCommand(hotelId, number, userId, nonceFromClient!, request));

            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
