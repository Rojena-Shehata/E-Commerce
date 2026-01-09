using E_Commerce.ServicesAbstraction.AdminDashboard.Abstractions;
using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Dashboard.Controllers
{
    public class UsersController(IUserService _userService) : Controller
    {
        [Authorize(Permission.Users.View)]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
    }
}
