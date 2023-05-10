﻿namespace JWTAuthentication.API.Models
{
    public class CustomResponseModel <T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
