using Api.Contracts.Order;
using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using Core.Entities;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderController(
            ISender sender,
            IHttpContextAccessor contextAccessor
        )
        {
            _sender = sender;
            _contextAccessor = contextAccessor;
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
        [HttpGet("user-orders")]
        [Authorize]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetUserOrders([FromQuery] GetOrdersRequest request)
        {
            string? userId = _contextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var userOrders = await _sender.Send(new GetUserOrdersQuery(userId!, request));

            var userOrdersDTO = userOrders.Select(x =>
                new UserOrdersDTO
                (
                    x.OrderId,
                    x.HotelName,
                    x.RoomNumber,
                    x.OverallPrice,
                    x.MinimarIncluded,
                    x.MealsIncluded,
                    x.CheckIn,
                    x.CheckOut,
                    x.Address
                )
            ).ToList();

            return Ok(new GetUserOrdersResponse(userOrdersDTO));
        }
    }
}
