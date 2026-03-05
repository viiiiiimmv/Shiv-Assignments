namespace IEnumberable;

class Employee
{
    public string Name { get;  }
    public int ID { get;  }
    public DateTime HireDate { get; }

    public Employee(string name, int id, DateTime hireDate)
    {
        Name = name;
        ID = id;
        HireDate = hireDate;
    }
}

class Program
{
    static void Main()
    {
        IEnumerable<Employee> employees = new List<Employee>()
        {
            new("Shiv", 8, new DateTime(2026, 08, 08)),
            new("Karan", 29, new DateTime(2026, 07, 05)),
            new("Utkarsh", 16, new DateTime(2026, 08, 08))
        };
        
        IEnumerable<Employee> query = from e in employees where e.HireDate.Year < 2027 orderby e.Name select e;
        // employees.Add(new Employee ("Linda", 4,  new DateTime(2026, 12, 08)));
        // -- This one will not work as IEnumerables are read-only objects
        foreach (Employee employee in query)
        {
            Console.WriteLine($"{employee.Name}\t{employee.ID}\t{employee.HireDate}");
        }
        
        
    }
}
