namespace LINQ_TO_EMPLOYEE_OBJECT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employees = EmployeeRepository.Retrive();
            var employees_2 = from emp in EmployeeRepository.Retrive() select emp;

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.EmployeeID}\t{emp.FirstName} {emp.LastName}\t{emp.City}\t{emp.Sal}");
            }

            // Sort by city followed by salary

            Console.WriteLine("\n Sort by city followed by salary : ");

            var usingOrderByThen = employees.OrderBy(x => x.City).ThenBy(x => x.Sal);
            var usingOrderByThen_2 = from emp in employees_2 orderby emp.City, emp.Sal select emp;

            foreach (var emp in usingOrderByThen)
            {
                Console.WriteLine($"{emp.EmployeeID}\t{emp.FirstName} {emp.LastName}\t{emp.City}\t{emp.Sal}");
            }

            Console.WriteLine();

            foreach (var emp in usingOrderByThen_2)
            {
                Console.WriteLine($"{emp.EmployeeID}\t{emp.FirstName} {emp.LastName}\t{emp.City}\t{emp.Sal}");
            }

            // First name and city
            var firstNameAndCity = from emp in employees
                                   select new
                                   {
                                       emp.FirstName,
                                       emp.City
                                   };

            var firstNameAndCity_2 = from emp in employees
                                     select new
                                     {
                                         fname = emp.FirstName,
                                         cityname = emp.City
                                     };

            foreach (var emp in firstNameAndCity)
            {
                Console.WriteLine($"{emp.FirstName}\t{emp.City}");
            }

            Console.WriteLine("\n");
            foreach (var emp in firstNameAndCity_2)
            {
                Console.WriteLine($"{emp.fname}\t{emp.cityname}");
            }

            // Full name display
            var fullName = from emp in employees
                           select new
                           {
                               fullname = emp.FirstName + " " + emp.LastName
            };

            foreach (var emp in fullName)
            {
                Console.WriteLine(emp.fullname);
            }

            // skip first 2 records and select 3 next records

            var fewRecordsSkipping = employees.Skip(2).Take(3);
            foreach(var emp in fewRecordsSkipping)
            {
                Console.WriteLine($"{emp.EmployeeID}\t{emp.FirstName} {emp.LastName}\t{emp.City}\t{emp.Sal}");
            }


            //fetch employee details using emp id

            Console.Write("Enter the employee ID : ");
            int reqID = Convert.ToInt32(Console.ReadLine());
            var fetchPerson = from emp in employees where emp.EmployeeID == reqID select emp;

            foreach (var emp in fetchPerson)
            {
                Console.WriteLine($"\nFullname:{emp.FirstName} {emp.LastName}\nCity:{emp.City}\nSalary:{emp.Sal}");
            }

            var groupCities = employees.GroupBy(x => x.City);
            foreach(var group in groupCities)
            {
                Console.WriteLine($"There are {group.Count()} employees in {group.Key}");
                Console.WriteLine("***************************************************");
                Console.WriteLine($"{group.Key} {group.Sum(x => x.Sal)}");
                foreach(var emp in group)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName}\t{emp.Sal}");   
                }
            }



        }
    }
}
