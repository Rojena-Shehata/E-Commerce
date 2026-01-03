using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Shared.Constants;
using E_Commerce.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser>userManager, 
                                        RoleManager<IdentityRole> roleManager,
                                        ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {

                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
                }

                if(! await _userManager.Users.AnyAsync())
                {
                    var user01 = new ApplicationUser()
                    {
                        DisplayName="Rojena Shehata",
                        UserName="Rojena_Shehata",
                        PhoneNumber="01213329698",
                        Email="rojenashehata@gmail.com"
                    };
                    var user02 = new ApplicationUser()
                    {
                        DisplayName="Dina Shehata",
                        UserName= "Dina_Shehata",
                        PhoneNumber="01213449698",
                        Email= "dinashehata@gmail.com"
                    };
                    await _userManager.CreateAsync(user01,"P@ssw0rd");
                    await _userManager.CreateAsync(user02,"P@ssw0rd");

                    await _userManager.AddToRolesAsync(user01, [Roles.SuperAdmin.ToString(),Roles.Admin.ToString(),Roles.Basic.ToString()]);
                    await _userManager.AddToRoleAsync(user02, Roles.Admin.ToString());

                }

                // SeedClaims
                await SeedClaimsForSuperAdminAsync(_roleManager);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding Identity Database : Message = {ex.Message}");
            }
        }

        private  async Task SeedClaimsForSuperAdminAsync( RoleManager<IdentityRole> roleManager)
        {
            var superAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            if(superAdminRole is not null)
            {
               await AddPermissionClaimsAsync(roleManager,superAdminRole);
            }
        }

        private  async Task AddPermissionClaimsAsync( RoleManager<IdentityRole> roleManager,IdentityRole role)
        {
            var allClaims=await roleManager.GetClaimsAsync(role);
           var allPermissions= Permission.GenerateAllPermissions();
            
            foreach (var permission in allPermissions)
            {
                if (permission is not null)
                    if (!allClaims.Any(claim => claim.Type == Permission.PermissionType && claim.Value == permission))
                        await roleManager.AddClaimAsync(role, new Claim(Permission.PermissionType, permission));

            }
        }

    }
}
