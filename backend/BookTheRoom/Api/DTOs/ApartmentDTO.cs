﻿using Core.Entities;

namespace Api.DTOs
{
    public record ApartmentDTO
    (
        int Id,
        string Title,
        string Description,
        string Address,
        List<string>? Images,
        List<Comment>? Comments
    );
}
