using ims.Infrastructure.Identity;
using ims.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace ims.Infrastructure.SeedData;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[]
        {
            new ApplicationRole { Name = "SuperAdmin", Description = "Full system access" },
            new ApplicationRole { Name = "Admin", Description = "Administrative access" },
            new ApplicationRole { Name = "Manager", Description = "Manager access" },
            new ApplicationRole { Name = "Staff", Description = "Staff access" }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }

        await SeedRolePermissionsAsync(roleManager, "SuperAdmin", ims.Shared.Constants.Permissions.AllPermissions);
        await SeedRolePermissionsAsync(roleManager, "Admin", ims.Shared.Constants.Permissions.AllPermissions);
        await SeedRolePermissionsAsync(roleManager, "Manager", ims.Shared.Constants.Permissions.AllPermissions.Where(p => p.Contains(".View") || p.Contains(".Create") || p.Contains(".Update")).ToArray());
        await SeedRolePermissionsAsync(roleManager, "Staff", ims.Shared.Constants.Permissions.AllPermissions.Where(p => p.Contains(".View")).ToArray());

        var adminEmail = "admin@ims.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(admin, "Admin@12345");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
        }
    }

    private static async Task SeedRolePermissionsAsync(
        RoleManager<ApplicationRole> roleManager,
        string roleName,
        IEnumerable<string> permissionsToSeed)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null) return;

        var existingClaims = await roleManager.GetClaimsAsync(role);
        var existingValues = existingClaims
            .Where(c => c.Type == AppConstants.PermissionClaimType)
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var permission in permissionsToSeed.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!existingValues.Contains(permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(AppConstants.PermissionClaimType, permission));
            }
        }
    }
}
