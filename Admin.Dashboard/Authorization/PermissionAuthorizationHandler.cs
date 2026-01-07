using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Admin.Dashboard.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
    
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            //Can access condition
            if (context.User.Claims.Any(claim=>claim.Type==Permission.PermissionType&&claim.Value==requirement.Permission&&claim.Issuer=="LOCAL AUTHORITY") )
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
