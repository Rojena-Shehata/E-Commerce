using E_Commerce.Domain.Contracts;
using E_Commerce.Presistence.IdentityData.DbContexts;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_Commerce.Shared.Constants.Permission;

namespace E_Commerce.Presistence.Repositories
{
    public class UserRepository(StoreIdentityDbContext _dbContext) : IUserRepository
    {
        public async Task<object> GetAllUsersWithRolesAsync() 
        {
          return await (from user in _dbContext.Users.AsNoTracking()
                        .Where(user=>user.HasAdminAccount)
                             join userRole in _dbContext.UserRoles.AsNoTracking()
                            on user.Id equals userRole.UserId into userRoles
                             from ur in userRoles.DefaultIfEmpty()
                             join role in _dbContext.Roles.AsNoTracking()
                             on ur.RoleId equals role.Id into roles
                             from role in roles.DefaultIfEmpty()
                             let userWithRole = new
                             {
                                 Id = user.Id,
                                 DisplayName = user.DisplayName,

                                 Email = user.Email ?? "",
                                 UserName = user.UserName ?? "",
                                 RoleName = role.Name
                             }
                             group userWithRole by new { userWithRole.Id, userWithRole.DisplayName, userWithRole.Email, userWithRole.UserName }
                               into groupedUser
                        select new UserViewModel
                        {
                            Id = groupedUser.Key.Id,
                            DisplayName = groupedUser.Key.DisplayName,
                            UserName = groupedUser.Key.UserName,
                            Email = groupedUser.Key.Email,
                            Roles = groupedUser.Select(role => role.RoleName)
                        }
                             ).ToListAsync();

        }
    }
}
