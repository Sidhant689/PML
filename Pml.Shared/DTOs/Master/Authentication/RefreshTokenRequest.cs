﻿namespace Pml.Shared.DTOs.Master.Authentication
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
