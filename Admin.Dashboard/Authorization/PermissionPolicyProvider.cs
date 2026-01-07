using E_Commerce.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Admin.Dashboard.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        //to be used in case the policy name doesn't match its expected pattern
        private DefaultAuthorizationPolicyProvider _backupPolicyProvider { get; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> authorizationOptions)
        {
            _backupPolicyProvider = new DefaultAuthorizationPolicyProvider(authorizationOptions);
        }
        ////IAuthorizationPolicyProvider needs to implement GetDefaultPolicyAsync 
        ////to provide an authorization policy for [Authorize] attributes without a policy name specified.
        Task<AuthorizationPolicy> IAuthorizationPolicyProvider.GetDefaultPolicyAsync()
        {
            return _backupPolicyProvider.GetDefaultPolicyAsync();
        }

        Task<AuthorizationPolicy?> IAuthorizationPolicyProvider.GetFallbackPolicyAsync()
        {
            return _backupPolicyProvider.GetFallbackPolicyAsync();
        }

        //// GetPolicyAsync method can be updated to use the BackupPolicyProvider instead of returning null:
        
        Task<AuthorizationPolicy?> IAuthorizationPolicyProvider.GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(Permission.PermissionType, StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }
           return _backupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
