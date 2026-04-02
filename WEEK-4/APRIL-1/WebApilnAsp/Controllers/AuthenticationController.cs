using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApilnAsp.Models;
using WebApilnAsp.Security;
using WebApilnAsp.Services;

namespace WebApilnAsp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        return ToActionResult(await _authService.LoginAsync(loginModel));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        return ToActionResult(await _authService.RegisterAsync(registerUser));
    }

    [HttpGet("me")]
    [Authorize(Policy = AppPolicies.AuthenticatedUser)]
    public async Task<IActionResult> GetCurrentUser()
    {
        return ToActionResult(await _authService.GetCurrentUserAsync(User));
    }

    [HttpGet("roles")]
    [Authorize(Policy = AppPolicies.AuthenticatedUser)]
    public async Task<IActionResult> GetAvailableRoles()
    {
        return ToActionResult(await _authService.GetAvailableRolesAsync());
    }

    [HttpPost("users")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        return ToActionResult(await _authService.CreateUserAsync(request));
    }

    [HttpGet("users/{userId}/roles")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        return ToActionResult(await _authService.GetUserByIdAsync(userId));
    }

    [HttpPut("users/{userId}/roles")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> AssignRoles(string userId, [FromBody] AssignUserRolesRequest request)
    {
        return ToActionResult(await _authService.AssignRolesAsync(userId, request));
    }

    private IActionResult ToActionResult<T>(ServiceResult<T> result)
    {
        if (result.Data is null)
        {
            return StatusCode(result.StatusCode, new Response
            {
                Status = result.Status,
                Message = result.Message,
                Errors = result.Errors
            });
        }

        return StatusCode(result.StatusCode, new Response<T>
        {
            Status = result.Status,
            Message = result.Message,
            Errors = result.Errors,
            Data = result.Data
        });
    }
}
