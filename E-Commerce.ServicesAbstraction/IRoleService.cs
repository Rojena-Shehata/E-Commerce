using E_Commerce.Shared.AdminDashboardViewModels;
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
    }
}
