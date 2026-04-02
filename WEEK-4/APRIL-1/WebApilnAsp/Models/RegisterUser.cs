using System.ComponentModel.DataAnnotations;

namespace WebApilnAsp.Models;

public class RegisterUser
{
    [Required(ErrorMessage = "User Name is required")]
    [MinLength(3, ErrorMessage = "User Name must be at least 3 characters long")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string Password { get; set; } = string.Empty;
}
