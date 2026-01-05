using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.CommonResult;

namespace E_Commerce.ServicesAbstraction
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetAllRolesAsync();
        Task<Result> AddRoleAsync(RoleFormViewModel input);
        Task<Result<PermissionFormViewModel>> GetPermissionsForRoleAsync(string roleId);
        Task<Result> UpdatePermissionsForRoleAsync(PermissionFormViewModel input);
        Task<Result> DeleteRoleWithItsClaimsAsync(string roleId);
        Task<Result<UpdateRoleViewModel>> GetRoleForUpdateRoleAsync(string roleId);
        Task<Result> UpdateRoleAsync(UpdateRoleViewModel input);
    }
}
