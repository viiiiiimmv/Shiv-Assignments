using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WEBAPI_DEMO.Models;

public class EmployeeUpsertRequest
{
    [Required(ErrorMessage = "Please enter your firstname")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your lastname")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Please enter email id")]
    [EmailAddress(ErrorMessage = "Please enter valid email id")]
    public string? Email { get; set; }

    [Range(1, 100, ErrorMessage = "Please enter your age between 1 to 100 only")]
    public int Age { get; set; }

    public IFormFile? Image { get; set; }
}
