using E_Commerce.Services;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.AdminDashboardViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class RolesController : MvcBaseController
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
        [HttpPost]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("index", await _roleService.GetAllRolesAsync());
            var result =await _roleService.AddRoleAsync(model);
            if (result.IsSucceed)
            {
                TempData["SuccessMessage"] = "Role Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            HandleModelErrors(ModelState, result);
            return View("index",await _roleService.GetAllRolesAsync());
        }
    }
}
