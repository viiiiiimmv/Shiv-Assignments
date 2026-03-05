/*
This coding question evaluates inheritance and salary calculation.

Requirements:

1. Create an abstract Employee class.
2. Include properties: Id, Name, BaseSalary.
3. Add abstract method CalculateSalary().
4. Create:
     - Manager (adds bonus)
     - Developer (adds project allowance)
5. Override salary logic in each.
6. Print salaries in Main().
*/

using System;

namespace EmployeeInheritance
{
     public abstract class Employee
     {
          public int Id { get; set; }
          public string Name { get; set; }
          public double BaseSalary { get; set; }

          protected Employee(int id, string name, double baseSalary)
          {
               Id = id;
               Name = name;
               BaseSalary = baseSalary;
          }

          public abstract double CalculateSalary();
     }

     public class Manager : Employee
     {
          public double Bonus { get; set; }

          public Manager(int id, string name, double baseSalary, double bonus)
               : base(id, name, baseSalary)
          {
               Bonus = bonus;
          }

          public override double CalculateSalary()
          {
               // TODO
               return BaseSalary + Bonus ;
          }
     }

     public class Developer : Employee
     {
          public double ProjectAllowance { get; set; }

          public Developer(int id, string name, double baseSalary, double allowance)
               : base(id, name, baseSalary)
          {
               ProjectAllowance = allowance;
          }

          public override double CalculateSalary()
          {
               // TODO
               return BaseSalary + ProjectAllowance;
          }
     }

     class Program
     {
          static void Main(string[] args)
          {
               Employee manager = new Manager(1, "Amit", 50000, 10000);
               Employee developer = new Developer(2, "Riya", 40000, 8000);

               Console.WriteLine(manager.Name + " Salary: " + manager.CalculateSalary());
               Console.WriteLine(developer.Name + " Salary: " + developer.CalculateSalary());

               Console.ReadLine();
          }
     }
}