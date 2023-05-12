using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JWTAuthentication.API.Interfaces
{
    public interface ICustomResponseModel <T> : IActionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
