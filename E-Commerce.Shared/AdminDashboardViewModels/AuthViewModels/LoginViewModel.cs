using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength:150,MinimumLength = 8,ErrorMessage = "Password length must be 8–150 characters.")]
        public string Password { get; set; }= default!; 
        public bool RememberMe {  get; set; }
    }
}
