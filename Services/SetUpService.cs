using JWTAuthentication.API.Controllers;
using JWTAuthentication.API.Database.Context;
using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using JWTAuthentication.API.Models.SetUpModels;
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

        public async Task<ICustomResponseModel<List<CustomRoleAndIdModel>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();

                if (roles.Count <= 0)
                    return new CustomResponseModel<List<CustomRoleAndIdModel>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "No roles found!"
                    };

                var customRoleModels = new List<CustomRoleAndIdModel>();
                
                foreach (var role in roles)
                    customRoleModels.Add(new CustomRoleAndIdModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    });

                return new CustomResponseModel<List<CustomRoleAndIdModel>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = customRoleModels
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<List<CustomRoleAndIdModel>>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<ICustomResponseModel<bool>> CreateRole(CreateRoleModel model)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation($"role created: {model.RoleName}");
                        return new CustomResponseModel<bool>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = $"role: {model.RoleName} created successfully",
                            Result = true
                        };
                    }

                    _logger.LogError($"error while creating role: {model.RoleName}");
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"issue while trying to add new role: {model.RoleName}",
                        Result = false
                    };
                }

                _logger.LogError($"role already exists: {model.RoleName}");
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

        public async Task<ICustomResponseModel<bool>> AddUserToRole(RoleModifyToUserModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "unable to find user",
                        Result = false
                    };

                var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

                if (!roleExist)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "uanble to find role",
                        Result = false
                    };

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"{model.RoleName} was added to {model.Email}");
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Result = true
                    };
                }

                _logger.LogError($"unable to add {model.RoleName} to {model.Email}");
                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"unable to add {model.RoleName} to {model.Email}",
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

        public async Task<ICustomResponseModel<bool>> RemoveUserFromRole(RoleModifyToUserModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "unable to find user",
                        Result = false
                    };

                var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

                if (!roleExist)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "uanble to find role",
                        Result = false
                    };

                if (!(await _userManager.GetRolesAsync(user)).Contains(model.RoleName))
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "role is not connected to user",
                        Result = false
                    };

                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"{model.RoleName} was removed from {model.Email}");
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Result = true
                    };
                }

                _logger.LogError($"unable to remove {model.RoleName} from {model.Email}");
                return new CustomResponseModel<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"unable to remove {model.RoleName} from {model.Email}",
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

        public async Task<ICustomResponseModel<List<CustomRoleModel>>> GetUserRoles(GetUserRolesModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return new CustomResponseModel<List<CustomRoleModel>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "unable to find user"
                    };

                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    _logger.LogError($"something went wrong while getting roles for {model.Email}");
                    return new CustomResponseModel<List<CustomRoleModel>>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "something went wrong while getting roles"
                    };
                }

                var customRoleModels = new List<CustomRoleModel>();
                foreach (var role in roles)
                    customRoleModels.Add(new CustomRoleModel()
                    {
                        RoleName = role
                    });

                _logger.LogInformation($"succesfully got roles for {model.Email}");

                return new CustomResponseModel<List<CustomRoleModel>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = customRoleModels
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<List<CustomRoleModel>>()
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
