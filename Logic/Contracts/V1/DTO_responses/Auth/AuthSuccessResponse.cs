﻿using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
