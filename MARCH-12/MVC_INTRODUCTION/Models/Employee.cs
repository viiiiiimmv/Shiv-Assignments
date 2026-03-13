namespace MVC_INTRODUCTION.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
    public int Salary { get; set; }
    public string? ImageUrl { get; set; }
    public int DeptId { get; set; }
    public Department? Department { get; set; }
}