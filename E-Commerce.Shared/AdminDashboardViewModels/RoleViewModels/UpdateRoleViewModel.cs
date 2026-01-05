using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels
{
    public class UpdateRoleViewModel
    {
        [Required]
        public string RoleId { get; set; } = default!;
        [Required(ErrorMessage = "Role Name is Required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 200 characters.")]
        public string RoleName { get; set; }= default!;

    }
}
