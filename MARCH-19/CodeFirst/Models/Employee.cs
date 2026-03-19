using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Employee
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Age is required")]
    [Range(0,100,ErrorMessage = "Age must be between 0 and 100")]
    public int Age { get; set; }
}