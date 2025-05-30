﻿namespace Api.Contracts.Hotel
{
    public record UpdateHotelForm
    (
        string Name,
        string Description,
        int Rating,
        bool Pool,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        List<IFormFile>? Images
    );
}