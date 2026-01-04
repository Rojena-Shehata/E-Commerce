using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.AdminDashboardViewModels;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RoleManager<IdentityRole> roleManager,ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
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

        public async Task<Result> DeleteRoleWithItsClaimsAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return Result.Fail(Error.NotFound("RoleId.NotFound", "Role Id Is Not Found"));
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null)
                return Result.Fail(Error.NotFound("Role.NotFound", "Role Not Found"));
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            try
            {
                if(roleClaims is not null && roleClaims.Any())
                {
                    foreach(var claim in roleClaims)
                    {
                        if (claim is not null)
                            await _roleManager.RemoveClaimAsync(role,claim);
                    }
                }
                var result=await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return Result.Ok();
                return Result.Fail(result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList());
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed To Delete Role => {ex.Message}");
                return Result.Fail(Error.Failure("Error", "Faile To Delete Role"));
            }


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


        public async Task<Result<PermissionFormViewModel>> GetPermissionsForRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return Error.NotFound("Error", "Role Id Is Not Valid");
            var role=await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return Error.NotFound("Error", $"Role Not Found");
            var allClaims = Permission.GenerateAllPermissions();
            
            var roleClaims=await _roleManager.GetClaimsAsync(role);

            var permissionViewModel = new PermissionFormViewModel()
            {
                RoleId = roleId,
                RoleName = role.Name ?? "",

                RoleClaims = allClaims.Select(claim => new CheckBoxViewModel()
                                            {
                                                DisplayName = claim,
                                                IsSelected = roleClaims.Any(roleclaim => roleclaim.Type==Permission.PermissionType&&claim == roleclaim.Value)
                                            }).ToList()
            };
            return permissionViewModel;
        }


        public async Task<Result> UpdatePermissionsForRoleAsync(PermissionFormViewModel input)
        {
            if (input is null)
                return Result.Fail(Error.Validation("Error","Input Not Valid"));
            try
            {
                var role = await _roleManager.FindByIdAsync(input.RoleId);
                if (role is null)
                    return Result.Fail(Error.NotFound("Role.NotFound", "RoleId Is Not Found"));
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                if (roleClaims is not null && roleClaims.Any())
                {
                    foreach (var claim in roleClaims)
                        await _roleManager.RemoveClaimAsync(role, claim);
                }
                var allSelectedClaims = input.RoleClaims.Where(claim => claim.IsSelected).Select(c => c.DisplayName);
                foreach (var claim in allSelectedClaims)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionType, claim));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to Update Permission For Role=>{ex.Message}");
               
            }
            return Result.Fail(Error.Failure("Error", "Error.Failure(Failed to Update Permission For Role"));
        }
    }
}
