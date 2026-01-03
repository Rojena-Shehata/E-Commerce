using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.ServicesAbstraction
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetAllRolesAsync();
        Task<Result> AddRoleAsync(RoleFormViewModel input);
    }
}
