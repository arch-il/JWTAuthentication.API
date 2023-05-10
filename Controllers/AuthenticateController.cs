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

        [HttpPost("[action]")]
        [Route("LogIn")]
        public async Task<CustomResponseModel<LogInResponceData>> LogIn([FromBody] LogInModel logInModel) => await _service.LogIn(logInModel);

        [HttpPost("[action]")]
        [Route("Register")]
        public async Task<CustomResponseModel<bool>> Register([FromBody] RegisterModel registerModel) => await _service.Register(registerModel);

        [HttpPost("[action]")]
        [Route("RegisterAdmin")]
        public async Task<CustomResponseModel<bool>> RegisterAdmin([FromBody] RegisterModel registerModel) => await _service.RegisterAdmin(registerModel);
    }
}
