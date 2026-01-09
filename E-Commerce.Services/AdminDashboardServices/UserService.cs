using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.ServicesAbstraction.AdminDashboard.Abstractions;
using E_Commerce.Shared.AdminDashboardViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.AdminDashboardServices
{
    public class UserService(IUserRepository _userRepository) : IUserService
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
    }
}
