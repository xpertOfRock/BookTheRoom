using Api.Extensions;
using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using Core.Contracts;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(
            ISender sender,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _sender = sender;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        [HttpGet("client-token")]
        public async Task<IActionResult> GetClientToken()
        {
            var token = await _sender.Send(new GetClientTokenQuery());
            return Ok(token);
        }

        [HttpPost("{hotelId}/{number}")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Post(int hotelId, int number, [FromBody] CreateOrderRequest request)
        {
            string? userId =  null;

            if (User.Identity!.IsAuthenticated)
            {
                userId = _contextAccessor.HttpContext!.User.GetUserId();
            }
                     
            var result = await _sender.Send(new CreateOrderCommand(hotelId, number, userId, request));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
