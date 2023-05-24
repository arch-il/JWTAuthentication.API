using JWTAuthentication.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetUpController : ControllerBase
    {
        private readonly ISetUpService _service;

        public SetUpController(ISetUpService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<List<IdentityRole>>> GetAllRoles() => await _service.GetAllRoles();

        [HttpPost("[action]")]
        public async Task<ICustomResponseModel<bool>> CreateRole(string roleName) => await _service.CreateRole(roleName);

        [HttpPost("[action]")]
        public async Task<ICustomResponseModel<bool>> AddUserToRole(string email, string roleName) => await _service.AddUserToRole(email, roleName);

        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<IList<string>>> GetUserRoles(string email) => await _service.GetUserRoles(email);
    }
}
