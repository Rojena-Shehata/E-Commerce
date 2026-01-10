using E_Commerce.Shared.AdminDashboardViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.AdminDashboardViewModels
{
    public class UserFormViewModel:UserBaseViewModel
    {
        public List<CheckBoxViewModel> Roles { get; set; }
    }
}
