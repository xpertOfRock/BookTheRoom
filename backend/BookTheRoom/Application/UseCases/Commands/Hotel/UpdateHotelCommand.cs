﻿using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using MediatR;

namespace Application.UseCases.Commands.Hotel
{
    public class UpdateHotelCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateHotelRequest UpdateHotelRequest { get; set; }
        
        public UpdateHotelCommand(int id, UpdateHotelRequest request)
        {
            Id = id;
            UpdateHotelRequest = request;
        }
    }
}
