using Microsoft.AspNetCore.Identity;

namespace WebApilnAsp.Security;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Hr = "HR";

    public static readonly string[] All = [Admin, User, Hr];
}

public static class AppPolicies
{
    public const string AuthenticatedUser = "AuthenticatedUser";
    public const string AdminOnly = "AdminOnly";
    public const string EmployeeRead = "EmployeeRead";
    public const string EmployeeWrite = "EmployeeWrite";
}

public static class AppRoleSeeds
{
    public static IReadOnlyList<IdentityRole> Get()
    {
        return
        [
            new IdentityRole
            {
                Id = "a4d43d0f-bc06-45ef-a3c6-6e4ecb4cda10",
                Name = AppRoles.Admin,
                ConcurrencyStamp = "1",
                NormalizedName = AppRoles.Admin.ToUpperInvariant()
            },
            new IdentityRole
            {
                Id = "2db5c449-f4b7-42d2-8a9c-ff2432db2d16",
                Name = AppRoles.User,
                ConcurrencyStamp = "2",
                NormalizedName = AppRoles.User.ToUpperInvariant()
            },
            new IdentityRole
            {
                Id = "bc9fcf76-4514-4404-8af4-0594fe2ea98e",
                Name = AppRoles.Hr,
                ConcurrencyStamp = "3",
                NormalizedName = AppRoles.Hr.ToUpperInvariant()
            }
        ];
    }
}
