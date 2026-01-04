using E_Commerce.Services;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.AdminDashboardViewModels;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
                HandleSuccessMessage("Role Created Successfully");
                return RedirectToAction(nameof(Index));
            }

            HandleErrors(ModelState, result.Errors);
            return View("index",await _roleService.GetAllRolesAsync());
        }

        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var result=await _roleService.GetPermissionsForRoleAsync(roleId);
            if(result.IsSucceed)
                return View(result.Value);
            HandleErrors(ModelState, result.Errors);

                return RedirectToAction(nameof(Index));
            
            

        }
        [HttpPost]
        public async Task<IActionResult> UpdatePermissions(PermissionFormViewModel input)
        {
            var result=await _roleService.UpdatePermissionsForRoleAsync(input);
            if(result.IsSucceed)
            {
                HandleSuccessMessage("Role Permissions Updated Successfully.");
               return RedirectToAction(nameof(Index));
            } 
            
            HandleErrors(ModelState,result.Errors);
            if (IsValidationError)
                return View(input);
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result=await _roleService.DeleteRoleWithItsClaimsAsync(roleId);
            if (result.IsSucceed)
                HandleSuccessMessage("Role Deleted Successfully");
            else
                HandleErrors(ModelState, result.Errors);
            return RedirectToAction(nameof(Index));

        }

    }
}
