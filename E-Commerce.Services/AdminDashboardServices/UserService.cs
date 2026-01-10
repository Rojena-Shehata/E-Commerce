using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction.AdminDashboard.Abstractions;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.AdminDashboardViewModels.UserViewModels;
using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.AdminDashboardServices
{
    public class UserService(IUserRepository _userRepository,
                             UserManager<ApplicationUser> _userManager,
                             RoleManager<IdentityRole> _roleManager,
                             ILogger<UserService> _logger) : IUserService
    {
       

        public async Task<IEnumerable<UserViewModel>?> GetAllUsersAsync()
        {


            return await _userRepository.GetAllUsersWithRolesAsync() as IEnumerable<UserViewModel>;
            // var users = await _userManager.Users
            //                              .AsNoTracking()
            //                              .ToListAsync();
            //return await Task.WhenAll(  users.Select(async  user => new UserViewModel
            // {
            //     Id = user.Id,
            //     DisplayName = user.DisplayName,
            //     Email = user.Email ?? "",
            //     UserName = user.UserName ?? "",
            //     Roles = await _userManager.GetRolesAsync(user)
            // }));

        }

        public async Task<Result<UserFormViewModel>> GetUserForUpdateAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Error.NotFound("UserId.NotFound", "Invalid User Id!");

            var user=await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Error.NotFound("User.NotFound", "User Is Not Found");
            var userViewModel = new UserFormViewModel()
            {
                Id = userId,
                DisplayName = user.DisplayName,
                Email = user.Email ?? "",
                UserName = user.UserName ?? ""

            };
            var allRoles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            if (allRoles is null || !allRoles.Any())
                return userViewModel;
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles is not null && userRoles.Any())
            {
                userViewModel.Roles = allRoles.Select( role => new CheckBoxViewModel()
                {
                    DisplayName = role.Name??"",
                    IsSelected = userRoles.Any(userRole=>userRole== role.Name)
                }).ToList();
            }
            else
            {
                userViewModel.Roles = allRoles.Select(role => new CheckBoxViewModel()
                {
                    DisplayName = role.Name??"",
                    IsSelected = false
                }).ToList();
            }

                return userViewModel;


        }

        public async Task<Result> UpdateRolesForUserASync(UserFormViewModel input)
        {
            if (input is null)
                return Error.NotFound("Input.NotFound", "Input Is not valid");
            var userResult=await GetAndValidateUserAndUserIdByIdAsync(input.Id);
            if(userResult.IsFail)
                return userResult.Errors.ToList();
            var user = userResult.Value;
            if (input.Roles is null || input.Roles.Count() == 0)
                return Result.Ok();
            var oldUserRoles =await _userManager.GetRolesAsync(user);

            try
            {
                //Update
                IdentityResult? resultOfRemoveRoles = null;
                if (oldUserRoles is not null && oldUserRoles.Any())
                {
                     resultOfRemoveRoles = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
                }
               if (resultOfRemoveRoles is null || resultOfRemoveRoles.Succeeded)
                {
                     var resultOfAddRoles =  await _userManager.AddToRolesAsync(user, input.Roles.Where(role => role.IsSelected).Select(role => role.DisplayName));
                    if(resultOfAddRoles is not null&& resultOfAddRoles.Succeeded)
                        return Result.Ok();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update roles for user. UserId: {input.Id} , Error Message => {ex.Message}");
               
            }

            return Error.Failure("UpdateUserRoles", "An error occurred while updating user roles. Please try again later.");

        }


        public async Task<Result> DeleteUserAsync(string userId)
        {
            var userResult = await GetAndValidateUserAndUserIdByIdAsync(userId);
            if (userResult.IsFail)
                return userResult.Errors.ToList();
            var user = userResult.Value;
            //null user Was checked
            try
            {

               var identityResultOfDelete=await _userManager.DeleteAsync(user);
                if (identityResultOfDelete is not null && identityResultOfDelete.Succeeded)
                    return Result.Ok();
                if (identityResultOfDelete is null)
                    return Error.Failure("Delete.Fail", "Failt To Delete User");
                return identityResultOfDelete.Errors.Select(error=>Error.Forbidden(error.Code,error.Description)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete user: {userId} with message => {ex.Message}");
            }
            return Error.Failure("Delete.Fail", "Failt To Delete User");
        }





        #region Helper Method
        private async Task<Result<ApplicationUser>> GetAndValidateUserAndUserIdByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Error.NotFound("UserId.NotFound", "User Not Found");
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Error.NotFound("User.NotFound", "User Not Found");
            return user;
        }

        #endregion
    }
}
