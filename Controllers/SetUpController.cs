using JWTAuthentication.API.Database.Context;
using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetUpController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SetUpController> _logger;
        private readonly JWTAuthenticationContext _db;

        public SetUpController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<SetUpController> logger, JWTAuthenticationContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        // create models for parameters

        [HttpGet("[action]")]
        public IActionResult GetAllRoles() // rewrite using async logger and custom response
        {
            var roles = _roleManager.Roles.ToList();

            return Ok(roles);
        }

        [HttpPost("[action]")]
        // add custom reponse model return type
        public async Task<IActionResult> CreateRole(string roleName) // string roleName => enum (optional)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"role created: {roleName}");
                    return Ok(new { Result = $"role: {roleName} created successfully" });
                }
                
                _logger.LogError($"error while creating role: {roleName}");
                return BadRequest(new { Result = $"issue while trying to add new role: {roleName}" });
            }

            _logger.LogError($"role already exists: {roleName}");
            return BadRequest(new { Error = "Role already exists" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new { Error = "unable to find user" });

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
                return BadRequest(new { Error = "unable to find role" });

            var result = _userManager.AddToRoleAsync(user, roleName);

            if (result.IsCompletedSuccessfully)
            {
                _logger.LogInformation($"{roleName} was added to {email}");
                return Ok(new { Result = "role added succesfully" });
            }
            
            _logger.LogError($"unable to add {roleName} to {email}");
            return BadRequest(new { Error = "unable to add role to user" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new { Error = "unable to find user" });

            var roles = _userManager.GetRolesAsync(user);

            if (!roles.IsCompletedSuccessfully)
            {
                _logger.LogError($"something went wrong while getting roles for {email}");
                return BadRequest(new { Error = "something went wrong while getting roles" });
            }

            _logger.LogInformation($"succesfully got roles for {email}");
            return Ok(roles);
        }
    }
}
