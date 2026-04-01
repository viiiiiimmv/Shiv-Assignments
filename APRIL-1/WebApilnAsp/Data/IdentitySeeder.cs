using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApilnAsp.Security;

namespace WebApilnAsp.Data;

public static class IdentitySeeder
{
    public static async Task SeedIdentityAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var bootstrapOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<BootstrapAdminOptions>>()
            .Value;

        foreach (var roleName in AppRoles.All)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!createRoleResult.Succeeded)
            {
                var errors = string.Join(", ", createRoleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
            }
        }

        await SeedBootstrapAdminAsync(userManager, bootstrapOptions);
    }

    private static async Task SeedBootstrapAdminAsync(
        UserManager<IdentityUser> userManager,
        BootstrapAdminOptions options)
    {
        if (!options.Enabled)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(options.Username)
            || string.IsNullOrWhiteSpace(options.Email)
            || string.IsNullOrWhiteSpace(options.Password))
        {
            throw new InvalidOperationException(
                "BootstrapAdmin is enabled, but Username, Email, or Password is missing.");
        }

        var existingAdmins = await userManager.GetUsersInRoleAsync(AppRoles.Admin);
        if (existingAdmins.Count > 0)
        {
            return;
        }

        var configuredRoles = options.Roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (configuredRoles.Length == 0)
        {
            configuredRoles = [AppRoles.Admin, AppRoles.User];
        }

        var existingUserByEmail = await userManager.FindByEmailAsync(options.Email.Trim());
        var existingUserByUsername = await userManager.FindByNameAsync(options.Username.Trim());
        var user = existingUserByEmail ?? existingUserByUsername;

        if (user is null)
        {
            user = new IdentityUser
            {
                UserName = options.Username.Trim(),
                Email = options.Email.Trim(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await userManager.CreateAsync(user, options.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to create bootstrap admin user: {errors}");
            }
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var rolesToAdd = configuredRoles
            .Except(currentRoles, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (rolesToAdd.Length == 0)
        {
            return;
        }

        var addRolesResult = await userManager.AddToRolesAsync(user, rolesToAdd);
        if (!addRolesResult.Succeeded)
        {
            var errors = string.Join(", ", addRolesResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Failed to assign roles to bootstrap admin user: {errors}");
        }
    }
}
