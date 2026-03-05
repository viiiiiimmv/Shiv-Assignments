using System.Text.RegularExpressions;

namespace PROBLEM_3;

class Program
{
    public static void ValidateNumberPlate(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            Console.WriteLine("INVALID FORMAT - EMPTY INPUT");
            return;
        }

        if (number.Length != 12)
        {
            Console.WriteLine($"{number} : INVALID FORMAT - INAPPROPRIATE LENGTH");
            return;
        }

        for (int i = 0; i < number.Length; i++)
        {
            char c = number[i];

            if (i == 4 || i == 7)
            {
                if (c != ' ')
                {
                    Console.WriteLine($"{number} : INVALID FORMAT - SPACE EXPECTED");
                    return;
                }
            }
            else
            {
                if (!char.IsLetterOrDigit(c))
                {
                    Console.WriteLine($"{number} : INVALID FORMAT - INVALID CHARACTER");
                    return;
                }
            }
        }

        var pattern = @"^[A-Z]{2}\d{2}\s[A-Z]{2}\s\d{4}$";

        if (Regex.IsMatch(number, pattern))
            Console.WriteLine($"{number} : VALID FORMAT");
        else
            Console.WriteLine($"{number} : INVALID FORMAT");
    }

    static void Main(string[] args)
    {
        string str = Console.ReadLine();
        ValidateNumberPlate(str);
    }
}
