using System.Security.Claims;
using WebApilnAsp.Models;

namespace WebApilnAsp.Services;

public interface IAuthService
{
    Task<ServiceResult<TokenResponse>> LoginAsync(LoginModel loginModel);
    Task<ServiceResult<AuthenticatedUserResponse>> RegisterAsync(RegisterUser registerUser);
    Task<ServiceResult<AuthenticatedUserResponse>> CreateUserAsync(CreateUserRequest request);
    Task<ServiceResult<AuthenticatedUserResponse>> AssignRolesAsync(string userId, AssignUserRolesRequest request);
    Task<ServiceResult<AuthenticatedUserResponse>> GetUserByIdAsync(string userId);
    Task<ServiceResult<IReadOnlyCollection<string>>> GetAvailableRolesAsync();
    Task<ServiceResult<AuthenticatedUserResponse>> GetCurrentUserAsync(ClaimsPrincipal principal);
}
