using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using JWTAuthentication.API.Models.SetUpModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
        public async Task<ICustomResponseModel<List<CustomRoleAndIdModel>>> GetAllRoles() => await _service.GetAllRoles();

        [HttpPost("[action]")]
        public async Task<ICustomResponseModel<bool>> CreateRole([FromBody] CreateRoleModel model) => await _service.CreateRole(model);

        [HttpPost("[action]")]
        public async Task<ICustomResponseModel<bool>> AddUserToRole([FromBody] RoleModifyToUserModel model) => await _service.AddUserToRole(model);

        [HttpPost("[action]")]
        public async Task<ICustomResponseModel<bool>> RemoveUserFromRole([FromBody] RoleModifyToUserModel model) => await _service.RemoveUserFromRole(model);


        [HttpGet("[action]")]
        public async Task<ICustomResponseModel<List<CustomRoleModel>>> GetUserRoles(GetUserRolesModel model) => await _service.GetUserRoles(model);
    }
}
