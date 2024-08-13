﻿namespace Api.Contracts
{
    public record CreateHotelForm
    (
        string Name,
        string Description,
        int Rating,
        int Rooms,
        bool Pool,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        List<IFormFile> Images
    );
}