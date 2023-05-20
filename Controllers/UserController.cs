using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using JWTAuthentication.API.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<IEnumerable<ViewUserModel>>> GetAllUsers() => await _service.GetAllUsers(); // implement this function in service and interface


        [Authorize]
        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<ViewUserModel>> GetUserById(int id) => await _service.GetUserById(id);

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<ViewUserModel>> GetUserByName(string name) => await _service.GetUserByName(name);
    }
}
