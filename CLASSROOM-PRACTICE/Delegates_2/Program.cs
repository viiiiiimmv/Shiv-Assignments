namespace Delegates_2;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
GALACTIC HR SYSTEM EXERCISE
==========================================================

STORY
A Galactic Corporation evaluates employees working across
multiple planets. Different employee types receive bonuses
based on different rules.

Your task is to design the HR system using:
- OOP
- Delegates
- LINQ
- Functional Programming

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------
*/
public abstract class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public double Salary { get; set; }
    public int Rating { get; set; }

    public virtual double CalculateBonus()
    {
        return Salary * 0.10;
    }
}
/*
1. Create an abstract class Employee

Properties:
- int Id
- string Name
- string Department
- double Salary
- int Rating

Method:
- virtual double CalculateBonus()

RULES
- Default implementation should return 10% of Salary


----------------------------------------------------------

2. Create Derived Classes

Commander
Engineer
Intern

Override CalculateBonus()

Rules:

Commander
Bonus = Salary * 0.25

Engineer
Bonus = Salary * 0.15

Intern
Bonus = Salary * 0.05

*/
public class Commander : Employee
{
    public override double CalculateBonus()
    {
        return Salary * 0.25;
    }
}

public class Engineer : Employee
{
    public override double CalculateBonus()
    {
        return Salary * 0.15;
    }
}

public class Intern : Employee
{
    public override double CalculateBonus()
    {
        return Salary * 0.05;
    }
}
/*
----------------------------------------------------------

3. Create Delegate

public delegate double BonusPolicy(Employee emp);

This delegate represents dynamic bonus strategies.


----------------------------------------------------------
*/
public delegate double BonusPolicy(Employee emp); 
/*
4. Create HRSystem Class

Fields
- private List<Employee> employees

Methods

AddEmployee(Employee emp)

ApplyBonus(Employee emp, BonusPolicy policy)

Rules
- Use the delegate to compute bonus
- Return the final salary including bonus


FilterEmployees(Func<Employee,bool> filter)

Rules
- Use LINQ Where
- Return filtered list


PerformAction(Action<Employee> action)

Rules
- Execute action on every employee


GetEmployees()

Rules
- Return the employee list


----------------------------------------------------------

5. LINQ REPORTS

Inside HRSystem implement methods:

GetHighestSalaryPerDepartment()

Rules
- Use GroupBy
- Return employee with highest salary per department


GetAverageSalaryPerDepartment()

Rules
- Return anonymous object
- Department + AverageSalary


GetEmployeesWithHighRating()

Rules
- Rating > 4


GetTotalPayroll()

Rules
- Sum of all salaries
*/

public class DeptSalary
{
    public string Department { get; set; }
    public double AverageSalary { get; set; }
}
public class HRSystem
{
    private List<Employee> _employees = new List<Employee>();

    public void AddEmployee(Employee emp)
    {
        _employees.Add(emp);
    }

    public double ApplyBonus(Employee emp, BonusPolicy policy)
    {
        double finalSalary = emp.Salary + emp.CalculateBonus();

        foreach (BonusPolicy bp in policy.GetInvocationList())
        {
            finalSalary += bp(emp);
        }

        return finalSalary;
    }

    public List<Employee> FilterEmployees(Func<Employee, bool> filter)
    {
        return _employees.Where(filter).ToList();
    }

    public void PerformAction(Action<Employee> action)
    {
        _employees.ForEach(action);
    }

    public List<Employee> GetEmployees()
    {
        return _employees;
    }

    public List<Employee> GetHighestSalaryPerDepartment()
    {
        return _employees
            .GroupBy(e => e.Department)
            .Select(g => g.OrderByDescending(x => x.Salary).First()).ToList();
    }

    public List<DeptSalary> GetAverageSalaryPerDepartment()
    {
        return _employees
            .GroupBy(g => g.Department)
            .Select(x => new DeptSalary
            {
                Department = x.Key,
                AverageSalary = x.Average(p => p.Salary)
            })
            .ToList();
    }

    public List<Employee> GetEmployeesWithHighRating()
    {
        return _employees.Where(e => e.Rating > 4).ToList();
    }

    public double GetTotalPayroll()
    {
        return _employees.Sum(e => e.Salary);
    }
}
/*

----------------------------------------------------------

6. DO NOT MODIFY MAIN METHOD
Main is already implemented and assumes your
classes/methods work correctly.

==========================================================
*/

class Program
{
    static void Main()
    {
        HRSystem hr = new HRSystem();

        // Create employees
        Employee e1 = new Commander
        {
            Id = 1,
            Name = "Zorak",
            Department = "Defense",
            Salary = 9000,
            Rating = 5
        };

        Employee e2 = new Engineer
        {
            Id = 2,
            Name = "Lyra",
            Department = "Tech",
            Salary = 7000,
            Rating = 4
        };

        Employee e3 = new Engineer
        {
            Id = 3,
            Name = "Nova",
            Department = "Tech",
            Salary = 6500,
            Rating = 5
        };

        Employee e4 = new Intern
        {
            Id = 4,
            Name = "Milo",
            Department = "Research",
            Salary = 2000,
            Rating = 3
        };

        Employee e5 = new Commander
        {
            Id = 5,
            Name = "Karn",
            Department = "Defense",
            Salary = 10000,
            Rating = 4
        };

        // Add employees
        hr.AddEmployee(e1);
        hr.AddEmployee(e2);
        hr.AddEmployee(e3);
        hr.AddEmployee(e4);
        hr.AddEmployee(e5);

        //--------------------------------------------------
        // BONUS POLICIES
        //--------------------------------------------------

        BonusPolicy festivalBonus = emp => emp.Salary * 0.10;
        BonusPolicy loyaltyBonus = emp => emp.Salary * 0.05;

        BonusPolicy combined = festivalBonus + loyaltyBonus;

        Console.WriteLine("BONUS CALCULATIONS\n");

        foreach (var emp in hr.GetEmployees())
        {
            double newSalary = hr.ApplyBonus(emp, combined);

            Console.WriteLine($"{emp.Name} -> Final Salary: {newSalary}");
        }

        //--------------------------------------------------
        // FILTER EMPLOYEES
        //--------------------------------------------------

        Console.WriteLine("\nENGINEERS ONLY");

        var engineers = hr.FilterEmployees(e => e.Department == "Tech");

        foreach (var e in engineers)
        {
            Console.WriteLine(e.Name);
        }

        //--------------------------------------------------
        // ACTION EXAMPLE
        //--------------------------------------------------

        Console.WriteLine("\nPRINT EMPLOYEES");

        hr.PerformAction(e =>
        {
            Console.WriteLine($"{e.Name} | {e.Department} | {e.Salary}");
        });

        //--------------------------------------------------
        // LINQ REPORTS
        //--------------------------------------------------

        Console.WriteLine("\nHIGHEST SALARY PER DEPARTMENT");

        var highest = hr.GetHighestSalaryPerDepartment();

        foreach (var e in highest)
        {
            Console.WriteLine($"{e.Department} -> {e.Name} ({e.Salary})");
        }

        Console.WriteLine("\nAVERAGE SALARY PER DEPARTMENT");

        var avg = hr.GetAverageSalaryPerDepartment();

        foreach (var a in avg)
        {
            Console.WriteLine($"{a.Department} -> {a.AverageSalary}");
        }

        Console.WriteLine("\nHIGH RATING EMPLOYEES");

        var top = hr.GetEmployeesWithHighRating();

        foreach (var e in top)
        {
            Console.WriteLine(e.Name);
        }

        Console.WriteLine("\nTOTAL PAYROLL");

        Console.WriteLine(hr.GetTotalPayroll());
    }
}