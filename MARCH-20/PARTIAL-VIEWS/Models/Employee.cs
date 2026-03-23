using System.ComponentModel.DataAnnotations;

namespace MARCH_20.Models;

public class Employee
{
    [Key]
    public int EmpId { get; set; }
    public string? EmpName { get; set; }
    public string? Email  { get; set; }
    public string? Description { get; set; }
}