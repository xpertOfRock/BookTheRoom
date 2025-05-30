﻿using Newtonsoft.Json;

namespace Core.Contracts
{
    public record GetHotelsRequest
    (        
        string? Search,
        string? SortItem,
        string? SortOrder,
        string? Countries,
        string? Ratings,
        string? Services,
        float? UserScore,
        int ItemsCount = 9,
        int Page = 1        
    );
}
