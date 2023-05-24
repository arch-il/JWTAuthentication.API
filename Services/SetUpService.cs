using JWTAuthentication.API.Controllers;
using JWTAuthentication.API.Database.Context;
using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWTAuthentication.API.Services
{
    public class SetUpService : ISetUpService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SetUpController> _logger;
        private readonly JWTAuthenticationContext _db;

        public SetUpService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<SetUpController> logger, JWTAuthenticationContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _db = db;
        }

        public async Task<ICustomResponseModel<List<IdentityRole>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();

                return new CustomResponseModel<List<IdentityRole>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<List<IdentityRole>>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<ICustomResponseModel<bool>> CreateRole(string roleName)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation($"role created: {roleName}");
                        return new CustomResponseModel<bool>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = $"role: {roleName} created successfully",
                            Result = true
                        };
                    }

                    _logger.LogError($"error while creating role: {roleName}");
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"issue while trying to add new role: {roleName}",
                        Result = false
                    };
                }

                _logger.LogError($"role already exists: {roleName}");
                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "role already exists",
                    Result = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong",
                    Result = false
                };
            }
        }

        public async Task<ICustomResponseModel<bool>> AddUserToRole(string email, string roleName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "unable to find user",
                        Result = false
                    };

                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "uanble to find role",
                        Result = false
                    };

                var result = _userManager.AddToRoleAsync(user, roleName);

                if (result.IsCompletedSuccessfully)
                {
                    _logger.LogInformation($"{roleName} was added to {email}");
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Result = true
                    };
                }

                _logger.LogError($"unable to add {roleName} to {email}");
                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"unable to add {roleName} to {email}",
                    Result = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong",
                    Result = false
                };
            }
        }

        public async Task<ICustomResponseModel<IList<string>>> GetUserRoles(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return new CustomResponseModel<IList<string>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "unable to find user"
                    };

                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    _logger.LogError($"something went wrong while getting roles for {email}");
                    return new CustomResponseModel<IList<string>>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "something went wrong while getting roles"
                    };
                }

                _logger.LogInformation($"succesfully got roles for {email}");

                return new CustomResponseModel<IList<string>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<IList<string>>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
