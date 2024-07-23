﻿using Core.Contracts;
using Core.Enums;
using MediatR;

namespace Application.UseCases.Commands.Room
{
    public class CreateRoomCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int HotelId { get; set; }

        public RoomCategory Category { get; set; }
        public CreateRoomCommand(int hotelId, CreateRoomRequest request)
        {
            Name = request.Name;
            Description = request.Description;
            Number = request.Number;
            Price = request.Price;
            HotelId = hotelId;
            Category = request.Category;
        }
    }
}
