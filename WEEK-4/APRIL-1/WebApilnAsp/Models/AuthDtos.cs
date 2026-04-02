using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApilnAsp.Models;

public class CreateUserRequest : RegisterUser
{
    public List<string> Roles { get; set; } = [];
}

public class AssignUserRolesRequest
{
    [Required(ErrorMessage = "At least one role is required.")]
    [MinLength(1, ErrorMessage = "At least one role is required.")]
    public List<string> Roles { get; set; } = [];

    public bool ReplaceExistingRoles { get; set; } = true;
}

public class AuthenticatedUserResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
}

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public AuthenticatedUserResponse User { get; set; } = new();
}

public class Response<T> : Response
{
    public T? Data { get; set; }
}

public class ServiceResult<T>
{
    public int StatusCode { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public IEnumerable<string>? Errors { get; init; }
    public T? Data { get; init; }

    public static ServiceResult<T> Success(T data, string message, int statusCode = StatusCodes.Status200OK)
    {
        return new ServiceResult<T>
        {
            StatusCode = statusCode,
            Status = "Success",
            Message = message,
            Data = data
        };
    }

    public static ServiceResult<T> Failure(int statusCode, string message, IEnumerable<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            StatusCode = statusCode,
            Status = "Error",
            Message = message,
            Errors = errors?.ToArray()
        };
    }
}
