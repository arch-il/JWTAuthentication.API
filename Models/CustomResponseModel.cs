using JWTAuthentication.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace JWTAuthentication.API.Models
{
    public class CustomResponseModel <T> : ICustomResponseModel <T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        
        public async Task ExecuteResultAsync(ActionContext actionContext)
        {
            var response = actionContext.HttpContext.Response;

            response.ContentType = "application/json";

            response.StatusCode = Convert.ToInt32(StatusCode);

            var result = new
            {
                Message,
                Result
            };

            await response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
