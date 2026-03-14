using System.ComponentModel.DataAnnotations;

namespace CRUD_OPERATIONS.Models;

public class Dog
{
    public int ID { get; set; }
    
    [Required(ErrorMessage = "Name is required."), MaxLength(220)]
    public string Name { get; set; } = string.Empty;
    
    [Range(0, 20, ErrorMessage = "Age should be between 0 and 20.")]
    public int Age { get; set; }
    
    
    
}
