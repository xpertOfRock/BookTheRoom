﻿namespace Application.UseCases.Commands.Apartment
{
    public class UpdateApartmentCommand : ICommand<IResult>
    {
        public int Id { get; }
        public string UserId { get; }
        public UpdateApartmentRequest Request { get; }
        public UpdateApartmentCommand(int id, string userId, UpdateApartmentRequest request)
        {
            Id = id;
            UserId = userId;
            Request = request;
        }
    }
}
