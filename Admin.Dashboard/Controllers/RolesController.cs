using E_Commerce.Services;
using E_Commerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles=await _roleService.GetAllRolesAsync();
            return View(roles);
        }
    }
}
