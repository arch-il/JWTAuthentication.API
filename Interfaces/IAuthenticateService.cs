using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Interfaces
{
    public interface IAuthenticateService
    {
        Task<CustomResponseModel<LogInResponceData>> LogIn([FromBody] LogInModel logInModel);
        Task<CustomResponseModel<bool>> Register([FromBody] RegisterModel registerModel);
        Task<CustomResponseModel<bool>> RegisterAdmin([FromBody] RegisterModel registerModel);
    }
}
