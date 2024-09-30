using Api.Extensions;
using Application.UseCases.Commands.Order;
using Core.Contracts;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController
        (
            IMediator mediator,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }      

        [HttpPost("{hotelId}/{number}")]
        [AllowAnonymous]
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
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            return Ok();
        }
    }
}
