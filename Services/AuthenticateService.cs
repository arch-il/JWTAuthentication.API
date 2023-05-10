using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models;
using JWTAuthentication.API.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.API.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<CustomResponseModel<LogInResponceData>> LogIn(LogInModel logInModel)
        {
            var user = await _userManager.FindByNameAsync(logInModel.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, logInModel.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new CustomResponseModel<LogInResponceData>()
                {
                    StatusCode = 200,
                    Result = new LogInResponceData()
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    }
                };
            }

            return new CustomResponseModel<LogInResponceData>()
            {
                StatusCode = 401,
                Message = "Unauthorized"
            };
        }

        public async Task<CustomResponseModel<bool>> Register(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.Username);

            if (userExists != null)
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 403,
                    Message = "User already exists!",
                    Result = false
                };

            var applicationUser = new ApplicationUser()
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, registerModel.Password);

            if (!result.Succeeded)
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 424,
                    Message = "Failed to create user!",
                    Result = false
                };

            return new CustomResponseModel<bool>()
            {
                StatusCode = 200,
                Result = true
            };
        }

        public async Task<CustomResponseModel<bool>> RegisterAdmin(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.Username);

            if (userExists != null)
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 403,
                    Message = "User already exists!",
                    Result = false
                };

            var applicationUser = new ApplicationUser()
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser, registerModel.Password);

            if (!result.Succeeded)
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 424,
                    Message = "Failed to create user!",
                    Result = false
                };

            if (!await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.Admin)))
                await _roleManager.CreateAsync(new IdentityRole(Enum.GetName(UserRoles.Admin)));

            if (!await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.User)))
                await _roleManager.CreateAsync(new IdentityRole(Enum.GetName(UserRoles.User)));

            if (await _roleManager.RoleExistsAsync(Enum.GetName(UserRoles.Admin)))
                await _userManager.AddToRoleAsync(applicationUser, Enum.GetName(UserRoles.Admin));

            return new CustomResponseModel<bool>()
            {
                StatusCode = 200,
                Result = true
            };
        }
    }
}
