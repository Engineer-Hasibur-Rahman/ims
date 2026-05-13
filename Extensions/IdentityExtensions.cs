using ims.Shared.Constants;

namespace ims.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddPermissionPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var permission in Permissions.AllPermissions)
                {
                    options.AddPolicy(permission, policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(AppConstants.PermissionClaimType, permission);
                    });
                }
            });

            return services;
        }
    }
}
