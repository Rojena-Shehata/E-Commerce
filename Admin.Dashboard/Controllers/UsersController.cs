using E_Commerce.ServicesAbstraction.AdminDashboard.Abstractions;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Admin.Dashboard.Controllers
{
    [AutoValidateAntiforgeryToken] //Protect POST/PUT/DELETE form submissions from CSRF
    public class UsersController(IUserService _userService) : MvcBaseController
    {
        [Authorize(Permission.Users.View)]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }


        [Authorize(Permission.Users.Edit)]
        public async Task<IActionResult> Edit(string userId)
        {
            var result=await _userService.GetUserForUpdateAsync(userId);
            if(result.IsSucceed)
                return View(result.Value);
            HandleErrors(ModelState, result.Errors);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Permission.Users.Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("Edit",model);
            }
            var result=await _userService.UpdateRolesForUserASync(model);
            if(result.IsSucceed)
            {
                HandleSuccessMessage("User roles updated successfully.");
                return RedirectToAction(nameof(Index));
            }  
            HandleErrors(ModelState,result.Errors);
            if (IsValidationError)
                return View(model);
            else
                return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string userId)
        {
            var result=await _userService.DeleteUserAsync(userId);
            if(result.IsSucceed)
                HandleSuccessMessage("User Deleted Successfully");
            else
                HandleErrors(ModelState,result.Errors);
           return RedirectToAction(nameof(Index));

        }

    }
}
