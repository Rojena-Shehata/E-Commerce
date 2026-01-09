using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = default!;
        public string DisplayName { get; set; }= default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; }
        
        public IEnumerable<string?> Roles { get; set; } = [];
    }
}
