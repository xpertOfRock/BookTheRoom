using Api.Controllers;
using Application.DTOs;
using Application.UseCases.Commands.Order;
using Application.UseCases.Queries.Order;
using Core.Contracts;
using Core.Enums;
using Core.TasksResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tests.ControllersTests
{
    // Unit tests for OrderController.
    // Verifies authenticated vs anonymous order creation, user-orders endpoint,
    // and client-token pass-through.
    public class OrderControllerTests
    {
        private readonly Mock<IMediator> _sender = new();

        private OrderController BuildController(string? userId = null)
        {
            var claims = new List<Claim>();
            if (userId != null)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

            // An authenticationType of "TestAuth" makes IsAuthenticated = true.
            var identity = new ClaimsIdentity(claims,
                userId != null ? "TestAuth" : string.Empty);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var controller = new OrderController(_sender.Object, accessorMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            return controller;
        }

        private static CreateOrderRequest ValidRequest() => new CreateOrderRequest(
            NonceFromClient: "nonce-123",
            Email: "guest@example.com",
            Number: "+380000000000",
            FirstName: "John",
            LastName: "Doe",
            MinibarIncluded: false,
            MealsIncluded: false,
            CheckIn: DateTimeOffset.UtcNow.AddDays(1),
            CheckOut: DateTimeOffset.UtcNow.AddDays(4));

        // GET /api/order/client-token

        [Fact]
        public async Task GetClientToken_ReturnsOkWithToken()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetClientTokenQuery>(), default))
                   .ReturnsAsync("braintree-token-abc");

            var result = await BuildController().GetClientToken();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("braintree-token-abc", ok.Value);
        }

        // POST /api/order/{hotelId}/{number}

        [Fact]
        public async Task Post_WhenMediatorSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateOrderCommand>(), default))
                   .ReturnsAsync(new Success("42"));

            var result = await BuildController(userId: "user-001")
                .Post(1, 101, ValidRequest());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_WhenMediatorFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateOrderCommand>(), default))
                   .ReturnsAsync(new Fail("Room is already booked.", ErrorStatuses.ConflictError));

            var result = await BuildController(userId: "user-001")
                .Post(1, 101, ValidRequest());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_WhenAnonymousUser_SendsCommandWithNullUserId()
        {
            _sender.Setup(s => s.Send(
                       It.Is<CreateOrderCommand>(c => c.UserId == null), default))
                   .ReturnsAsync(new Success("1"));

            await BuildController(userId: null).Post(1, 101, ValidRequest());

            _sender.Verify(s => s.Send(
                It.Is<CreateOrderCommand>(c => c.UserId == null), default), Times.Once);
        }

        [Fact]
        public async Task Post_WhenAuthenticatedUser_SendsCommandWithUserId()
        {
            _sender.Setup(s => s.Send(
                       It.Is<CreateOrderCommand>(c => c.UserId == "user-abc"), default))
                   .ReturnsAsync(new Success("1"));

            await BuildController(userId: "user-abc").Post(1, 101, ValidRequest());

            _sender.Verify(s => s.Send(
                It.Is<CreateOrderCommand>(c => c.UserId == "user-abc"), default), Times.Once);
        }

        // GET /api/order/user-orders

        [Fact]
        public async Task GetUserOrders_WhenAuthenticated_ReturnsOkWithOrders()
        {
            var orders = new List<UserOrdersDTO>
            {
                new UserOrdersDTO(1, "Grand Hotel", 101, 315m, false, false,
                    DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(4),
                    "Kyiv, Ukraine", OrderStatus.Awaiting)
            };

            _sender.Setup(s => s.Send(It.IsAny<GetUserOrdersQuery>(), default))
                   .ReturnsAsync(orders);

            var controller = BuildController(userId: "user-001");
            var result = await controller.GetUserOrders(
                new GetOrdersRequest(null, null, null));

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetUserOrders_WhenNoUserId_ReturnsUnauthorized()
        {
            var controller = BuildController(userId: null);

            var result = await controller.GetUserOrders(
                new GetOrdersRequest(null, null, null));

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
