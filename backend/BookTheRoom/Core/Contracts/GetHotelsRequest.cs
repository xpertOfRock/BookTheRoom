﻿namespace Core.Contracts
{
    public record GetHotelsRequest
    (
        
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Countries,
        string? Ratings,
        int page = 1
    );
}
