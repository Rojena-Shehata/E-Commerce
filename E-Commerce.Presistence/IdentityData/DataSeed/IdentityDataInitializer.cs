using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
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

                    await _userManager.AddToRoleAsync(user01,"SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding Identity Dataase : Message = {ex.Message}");
            }
        }
    }
}
