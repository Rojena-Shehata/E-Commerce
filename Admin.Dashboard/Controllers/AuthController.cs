using E_Commerce.ServicesAbstraction.AdmainDashboardAbstractions;
using E_Commerce.Shared.AdminDashboardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{

    public class AuthController(IAuthServiceForDashoboard _authService) : MvcBaseController
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel input)
        {
            if (!ModelState.IsValid)
                return View(input);
                
            var result =await _authService.LoginAsync(input);
            if (result.IsSucceed)
                return RedirectToAction(nameof(Index), "Home");
            HandleValidationModelErrors(ModelState, result.Errors);
            return View(input);
        }

        public async Task<IActionResult> Logout()
        {
           await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
