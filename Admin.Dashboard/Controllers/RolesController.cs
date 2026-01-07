using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Dashboard.Controllers
{
    public class RolesController : MvcBaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Permission.Roles.View)]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return View(roles);
        }


        [Authorize(Permission.Roles.Create)]
        [HttpPost]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("index", await _roleService.GetAllRolesAsync());
            var result = await _roleService.AddRoleAsync(model);
            if (result.IsSucceed)
            {
                HandleSuccessMessage("Role Created Successfully");
                return RedirectToAction(nameof(Index));
            }

            HandleErrors(ModelState, result.Errors);
            return View("index", await _roleService.GetAllRolesAsync());
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var result = await _roleService.GetPermissionsForRoleAsync(roleId);
            if (result.IsSucceed)
                return View(result.Value);
            HandleErrors(ModelState, result.Errors);

            return RedirectToAction(nameof(Index));



        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> UpdatePermissions(PermissionFormViewModel input)
        {
            var result = await _roleService.UpdatePermissionsForRoleAsync(input);
            if (result.IsSucceed)
            {
                HandleSuccessMessage("Role Permissions Updated Successfully.");
                return RedirectToAction(nameof(Index));
            }

            HandleErrors(ModelState, result.Errors);
            if (IsValidationError)
                return View(input);
            else
                return RedirectToAction(nameof(Index));
        }


        [Authorize(Permission.Roles.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result = await _roleService.DeleteRoleWithItsClaimsAsync(roleId);
            if (result.IsSucceed)
                HandleSuccessMessage("Role Deleted Successfully");
            else
                HandleErrors(ModelState, result.Errors);
            return RedirectToAction(nameof(Index));

        }


        [Authorize(Permission.Roles.Edit)]
        public async Task<IActionResult> Edit(string roleId)
        {
            var result = await _roleService.GetRoleForUpdateRoleAsync(roleId);
            if (result.IsSucceed)
                return View(result.Value);
            HandleErrors(ModelState, result.Errors);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Permission.Roles.Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel input)
        {
            if (!ModelState.IsValid)
                return View(nameof(Edit), input);

            var result = await _roleService.UpdateRoleAsync(input);
            if (result.IsSucceed)
            {
                HandleSuccessMessage("Role Updated successfully.");
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(ModelState, result.Errors);
            if (IsValidationError)
                return View("Edit", input);
            return RedirectToAction(nameof(Index));
        }


    }
}
