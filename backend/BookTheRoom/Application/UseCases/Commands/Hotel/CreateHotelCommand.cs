﻿using Core.Contracts;
using Core.ValueObjects;
using MediatR;

namespace Application.UseCases.Commands.Hotel
{
    public class CreateHotelCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int RoomsAmount { get; set; }
        public bool HasPool { get; set; }

        public Address Address { get; set; }
        public List<string>? Images { get; set; }
        public CreateHotelCommand(CreateHotelRequest request)
        {
            Name = request.Name;
            Description = request.Description;
            Rating = request.Rating;
            HasPool = request.Pool;
            Address = request.Address;
            Images = request.Images;
        }
    }
}

