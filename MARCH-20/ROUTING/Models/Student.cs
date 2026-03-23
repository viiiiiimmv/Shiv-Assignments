using System.ComponentModel.DataAnnotations;

namespace ROUTING.Models;

public class Student
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Class { get; set; }
    public string? Email { get; set; }
}