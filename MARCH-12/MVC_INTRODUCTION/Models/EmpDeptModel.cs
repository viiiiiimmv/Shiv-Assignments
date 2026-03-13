namespace MVC_INTRODUCTION.Models;

public class EmpDeptModel
{
    public List<Department> _departments { get; set; }
    public Employee? _employee { get; set; }
    public DateTime _date { get; set; }
}