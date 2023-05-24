using Microsoft.AspNetCore.Identity;

namespace JWTAuthentication.API.Interfaces
{
    public interface ISetUpService
    {
        Task<ICustomResponseModel<List<IdentityRole>>> GetAllRoles();
        Task<ICustomResponseModel<bool>> CreateRole(string roleName);
        Task<ICustomResponseModel<bool>> AddUserToRole(string email, string roleName);
        Task<ICustomResponseModel<IList<string>>> GetUserRoles(string email);
    }
}
