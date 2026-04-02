using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApilnAsp.Models;
using WebApilnAsp.Security;

namespace WebApilnAsp.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<ServiceResult<TokenResponse>> LoginAsync(LoginModel loginModel)
    {
        var identifier = loginModel.Username.Trim();
        var user = await FindUserAsync(identifier);
        if (user is null)
        {
            return ServiceResult<TokenResponse>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid username/email or password.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, true);
        if (signInResult.IsLockedOut)
        {
            return ServiceResult<TokenResponse>.Failure(
                StatusCodes.Status423Locked,
                "This account is temporarily locked because of repeated failed login attempts.");
        }

        if (!signInResult.Succeeded)
        {
            return ServiceResult<TokenResponse>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid username/email or password.");
        }

        var roles = await GetSortedRolesAsync(user);
        var expiration = DateTime.UtcNow.AddHours(_jwtOptions.ExpiryHours);
        var token = BuildToken(BuildClaims(user, roles), expiration);

        return ServiceResult<TokenResponse>.Success(
            new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                User = BuildUserResponse(user, roles)
            },
            "Login successful.");
    }

    public Task<ServiceResult<AuthenticatedUserResponse>> RegisterAsync(RegisterUser registerUser)
    {
        return CreateUserInternalAsync(
            registerUser,
            [AppRoles.User],
            $"User '{registerUser.Username.Trim()}' registered successfully.");
    }

    public Task<ServiceResult<AuthenticatedUserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        var requestedRoles = request.Roles.Count == 0 ? [AppRoles.User] : request.Roles;

        return CreateUserInternalAsync(
            request,
            requestedRoles,
            $"User '{request.Username.Trim()}' created successfully.");
    }

    public async Task<ServiceResult<AuthenticatedUserResponse>> AssignRolesAsync(
        string userId,
        AssignUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status404NotFound,
                $"User with id '{userId}' was not found.");
        }

        var requestedRolesResult = await ResolveRolesAsync(request.Roles, false);
        if (requestedRolesResult.Data is null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                requestedRolesResult.StatusCode,
                requestedRolesResult.Message,
                requestedRolesResult.Errors);
        }

        var requestedRoles = requestedRolesResult.Data;
        var currentRoles = await GetSortedRolesAsync(user);
        var desiredRoles = request.ReplaceExistingRoles
            ? requestedRoles
            : currentRoles
                .Union(requestedRoles, StringComparer.OrdinalIgnoreCase)
                .OrderBy(role => role)
                .ToArray();

        var rolesToRemove = request.ReplaceExistingRoles
            ? currentRoles.Except(desiredRoles, StringComparer.OrdinalIgnoreCase).ToArray()
            : Array.Empty<string>();
        var rolesToAdd = desiredRoles.Except(currentRoles, StringComparer.OrdinalIgnoreCase).ToArray();

        if (rolesToRemove.Length > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                return ServiceResult<AuthenticatedUserResponse>.Failure(
                    StatusCodes.Status400BadRequest,
                    "Failed to remove existing roles from the user.",
                    removeResult.Errors.Select(error => error.Description));
            }
        }

        if (rolesToAdd.Length > 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                return ServiceResult<AuthenticatedUserResponse>.Failure(
                    StatusCodes.Status400BadRequest,
                    "Failed to assign one or more roles to the user.",
                    addResult.Errors.Select(error => error.Description));
            }
        }

        var updatedRoles = await GetSortedRolesAsync(user);
        var message = rolesToAdd.Length == 0 && rolesToRemove.Length == 0
            ? "No role changes were required."
            : $"Roles updated successfully for user '{user.UserName}'.";

        return ServiceResult<AuthenticatedUserResponse>.Success(
            BuildUserResponse(user, updatedRoles),
            message);
    }

    public async Task<ServiceResult<AuthenticatedUserResponse>> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status404NotFound,
                $"User with id '{userId}' was not found.");
        }

        return ServiceResult<AuthenticatedUserResponse>.Success(
            BuildUserResponse(user, await GetSortedRolesAsync(user)),
            $"Roles loaded for user '{user.UserName}'.");
    }

    public async Task<ServiceResult<IReadOnlyCollection<string>>> GetAvailableRolesAsync()
    {
        var roles = await _roleManager.Roles
            .OrderBy(role => role.Name)
            .Select(role => role.Name!)
            .ToArrayAsync();

        return ServiceResult<IReadOnlyCollection<string>>.Success(
            roles,
            "Available roles loaded successfully.");
    }

    public async Task<ServiceResult<AuthenticatedUserResponse>> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status401Unauthorized,
                "Unable to determine the current user.");
        }

        return await GetUserByIdAsync(userId);
    }

    private async Task<ServiceResult<AuthenticatedUserResponse>> CreateUserInternalAsync(
        RegisterUser request,
        IEnumerable<string> requestedRoles,
        string successMessage)
    {
        var username = request.Username.Trim();
        var email = request.Email.Trim();

        if (await _userManager.FindByNameAsync(username) is not null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status409Conflict,
                $"Username '{username}' is already taken.");
        }

        if (await _userManager.FindByEmailAsync(email) is not null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status409Conflict,
                $"Email '{email}' is already registered.");
        }

        var resolvedRolesResult = await ResolveRolesAsync(requestedRoles, true);
        if (resolvedRolesResult.Data is null)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                resolvedRolesResult.StatusCode,
                resolvedRolesResult.Message,
                resolvedRolesResult.Errors);
        }

        var user = new IdentityUser
        {
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = username
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status400BadRequest,
                "User creation failed.",
                createResult.Errors.Select(error => error.Description));
        }

        var addRoleResult = await _userManager.AddToRolesAsync(user, resolvedRolesResult.Data);
        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return ServiceResult<AuthenticatedUserResponse>.Failure(
                StatusCodes.Status400BadRequest,
                "User was created, but assigning roles failed.",
                addRoleResult.Errors.Select(error => error.Description));
        }

        return ServiceResult<AuthenticatedUserResponse>.Success(
            BuildUserResponse(user, resolvedRolesResult.Data),
            $"{successMessage} Roles: {string.Join(", ", resolvedRolesResult.Data)}.",
            StatusCodes.Status201Created);
    }

    private async Task<ServiceResult<IReadOnlyCollection<string>>> ResolveRolesAsync(
        IEnumerable<string>? requestedRoles,
        bool allowDefaultUserRole)
    {
        var sanitizedRoles = requestedRoles?
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? Array.Empty<string>();

        if (sanitizedRoles.Length == 0)
        {
            if (!allowDefaultUserRole)
            {
                return ServiceResult<IReadOnlyCollection<string>>.Failure(
                    StatusCodes.Status400BadRequest,
                    "At least one valid role is required.");
            }

            sanitizedRoles = [AppRoles.User];
        }

        var normalizedRoles = sanitizedRoles.Select(role => role.ToUpperInvariant()).ToArray();
        var matchingRoles = await _roleManager.Roles
            .Where(role => normalizedRoles.Contains(role.NormalizedName!))
            .ToListAsync();

        var missingRoles = normalizedRoles
            .Except(matchingRoles.Select(role => role.NormalizedName!), StringComparer.Ordinal)
            .Select(missingRole => sanitizedRoles.First(role => string.Equals(
                role,
                missingRole,
                StringComparison.OrdinalIgnoreCase)))
            .OrderBy(role => role)
            .ToArray();

        if (missingRoles.Length > 0)
        {
            return ServiceResult<IReadOnlyCollection<string>>.Failure(
                StatusCodes.Status400BadRequest,
                $"The following roles are invalid: {string.Join(", ", missingRoles)}.",
                [$"Available roles: {string.Join(", ", AppRoles.All)}"]);
        }

        return ServiceResult<IReadOnlyCollection<string>>.Success(
            matchingRoles
                .Select(role => role.Name!)
                .OrderBy(role => role)
                .ToArray(),
            "Roles resolved successfully.");
    }

    private async Task<IdentityUser?> FindUserAsync(string identifier)
    {
        var trimmedIdentifier = identifier.Trim();
        var user = await _userManager.FindByNameAsync(trimmedIdentifier);

        return user ?? await _userManager.FindByEmailAsync(trimmedIdentifier);
    }

    private ClaimsIdentity BuildClaims(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }

        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken BuildToken(ClaimsIdentity claimsIdentity, DateTime expiration)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        return new JwtSecurityToken(
            issuer: _jwtOptions.ValidIssuer,
            audience: _jwtOptions.ValidAudience,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            claims: claimsIdentity.Claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
    }

    private AuthenticatedUserResponse BuildUserResponse(IdentityUser user, IReadOnlyCollection<string> roles)
    {
        return new AuthenticatedUserResponse
        {
            UserId = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles
        };
    }

    private async Task<IReadOnlyCollection<string>> GetSortedRolesAsync(IdentityUser user)
    {
        return (await _userManager.GetRolesAsync(user))
            .OrderBy(role => role)
            .ToArray();
    }
}
