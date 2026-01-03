using E_Commerce.ServicesAbstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Shared.AdminDashboardViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Shared.CommonResult;

namespace E_Commerce.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result> AddRoleAsync(RoleFormViewModel input)
        {
            if (input is null)
                return Result.Fail(Error.Validation("Error", "Invalid Null Input"));
            if ( await _roleManager.RoleExistsAsync(input.Name))
            {
                return Result.Fail(Error.Validation("Error", "Role already exists."));
            }
           var result= await _roleManager.CreateAsync(new IdentityRole(input.Name.Trim()));
            
            if (result.Succeeded)
                return Result.Ok();
            
           return Result.Fail( result.Errors.Select(e=>Error.Validation(e.Code,e.Description)).ToList());
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRolesAsync()
        {
            var roles =await _roleManager.Roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name??""
            }).ToListAsync();
            return roles;

        }
    }
}
