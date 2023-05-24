using JWTAuthentication.API.Interfaces;
using JWTAuthentication.API.Models.UserModels;
using JWTAuthentication.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CustomResponseModel<IEnumerable<ViewUserModel>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null)
                return new CustomResponseModel<IEnumerable<ViewUserModel>>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "no users found"
                };

            var viewUserModels = new List<ViewUserModel>();
            foreach (var user in users)
            {
                viewUserModels.Add(new ViewUserModel()
                {
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            
            return new CustomResponseModel<IEnumerable<ViewUserModel>>()
            {
                StatusCode = HttpStatusCode.OK,
                Result = viewUserModels
            };
        }


        public async Task<CustomResponseModel<ViewUserModel>> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            if (user == null)
                return new CustomResponseModel<ViewUserModel>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Not found!"
                };

            var viewUserModel = new ViewUserModel()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return new CustomResponseModel<ViewUserModel>()
            {
                StatusCode = HttpStatusCode.OK,
                Result = viewUserModel
            };
        }

        public async Task<CustomResponseModel<ViewUserModel>> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
                return new CustomResponseModel<ViewUserModel>()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Not found!"
                };

            var viewUserModel = new ViewUserModel()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return new CustomResponseModel<ViewUserModel>()
            {
                StatusCode = HttpStatusCode.OK,
                Result = viewUserModel
            };
        }
    }
}
