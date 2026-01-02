using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels
{
    public record RoleViewModel
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; }=default!;
    }
}
