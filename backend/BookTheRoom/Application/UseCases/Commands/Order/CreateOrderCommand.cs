﻿using Core.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Order
{
    public class CreateOrderCommand : IRequest<Unit>
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public string? UserId { get; set; }
        public CreateOrderRequest Request { get; set; }
        public CreateOrderCommand(int hotelId, int number, string? userId, CreateOrderRequest request)
        {
            HotelId = hotelId;
            Number = number;
            UserId = userId;
            Request = request;
        }
    }
}
