using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _service;

        public AuthenticateController(IAuthenticateService service)
        {
            _service = service;
        }

        [HttpPost("LogIn")]
        public async Task<ICustomResponseModel<LogInResponceData>> LogIn([FromBody] LogInModel logInModel) => await _service.LogIn(logInModel);

        [HttpPost("Register")]
        public async Task<ICustomResponseModel<bool>> Register([FromBody] RegisterModel registerModel) => await _service.Register(registerModel);

        [HttpPost("RegisterAdmin")]
        public async Task<ICustomResponseModel<bool>> RegisterAdmin([FromBody] RegisterModel registerModel) => await _service.RegisterAdmin(registerModel);
    }
}
