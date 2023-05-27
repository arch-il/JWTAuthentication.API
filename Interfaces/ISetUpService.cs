using JWTAuthentication.API.Models;
using JWTAuthentication.API.Models.SetUpModels;

namespace JWTAuthentication.API.Interfaces
{
    public interface ISetUpService
    {
        Task<ICustomResponseModel<List<CustomRoleAndIdModel>>> GetAllRoles();
        Task<ICustomResponseModel<bool>> CreateRole(CreateRoleModel model);
        Task<ICustomResponseModel<bool>> AddUserToRole(RoleModifyToUserModel model);
        Task<ICustomResponseModel<bool>> RemoveUserFromRole(RoleModifyToUserModel model);
        Task<ICustomResponseModel<List<CustomRoleModel>>> GetUserRoles(GetUserRolesModel model);
    }
}
