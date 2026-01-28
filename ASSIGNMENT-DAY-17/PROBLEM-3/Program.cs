using System.Text.RegularExpressions;

namespace PROBLEM_3;

class Program
{
    public static bool ValidateNumberPlate(string number)
    {
        var pattern = @"^[A-Z]{2}\d{2}[A-Z]{2}\d{4}$";
        return Regex.IsMatch(number, pattern);
    }
    static void Main(string[] args)
    {
        string str = Console.ReadLine();
        bool output =  ValidateNumberPlate(str);

        if (output)
        {
            Console.WriteLine($"{str} is valid");
        }
        else
        {
            Console.WriteLine($"{str} is not valid");
        }
    }
}