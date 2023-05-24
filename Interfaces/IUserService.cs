using JWTAuthentication.API.Models;
using JWTAuthentication.API.Models.UserModels;

namespace JWTAuthentication.API.Interfaces
{
    public interface IUserService
    {
        Task<CustomResponseModel<IEnumerable<ViewUserModel>>> GetAllUsers();
        Task<CustomResponseModel<ViewUserModel>> GetUserById(int id);
        Task<CustomResponseModel<ViewUserModel>> GetUserByName(string name);
    }
}
