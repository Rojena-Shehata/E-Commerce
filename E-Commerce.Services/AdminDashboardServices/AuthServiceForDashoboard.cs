using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction.AdmainDashboardAbstractions;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.AdminDashboardServices
{
    public class AuthServiceForDashoboard(UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager) : IAuthServiceForDashoboard
    {
        public async Task<Result> LoginAsync(LoginViewModel input)
        {
            if (input is null)
                return Result.Fail(Error.NotFound("Input.NotFound", "Input Not Found"));
            var user=await _userManager.FindByEmailAsync(input.Email);
            if(user is null)
                return Result.Fail(Error.InvalidCredentials("InvalidLogin", "Invalid email or password."));
             
            var signInResult=await _signInManager.PasswordSignInAsync(user,input.Password,input.RememberMe,true);////
            if (signInResult.Succeeded)
            {
                return Result.Ok();
            }
            
            if (signInResult.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                return Result.Fail(Error.UnAuthorized("",$"Your account is locked. Please try again on {lockoutEnd?.ToLocalTime():dd MMM yyyy - HH:mm tt}."));
            }

            if (signInResult.IsNotAllowed)
            {
                return Result.Fail(Error.UnAuthorized("","Login is not allowed. Please confirm your email or contact support."));
            }

            if (signInResult.RequiresTwoFactor)
            {
                return Result.Fail(Error.UnAuthorized("Two-factor authentication is required."));
            }
            return Result.Fail(Error.InvalidCredentials("", "Invalid email or password."));
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
