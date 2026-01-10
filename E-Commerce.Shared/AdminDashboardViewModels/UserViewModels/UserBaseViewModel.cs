using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels.UserViewModels
{
    public class UserBaseViewModel
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required]
        public string DisplayName { get; set; } = default!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string UserName { get; set; }
    }
}
