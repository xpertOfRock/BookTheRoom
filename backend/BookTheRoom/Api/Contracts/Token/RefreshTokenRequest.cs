﻿namespace Api.Contracts.Token
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }         
        public string RefreshToken { get; set; }  
    }
}
