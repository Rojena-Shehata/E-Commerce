using Microsoft.AspNetCore.Authorization;

namespace Admin.Dashboard.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get;  }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

    }
}
