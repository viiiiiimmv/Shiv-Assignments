using System.Text.RegularExpressions;
namespace REGEX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pattern = Console.ReadLine();
            var subject = Console.ReadLine();

            var regex = new Regex(pattern);

            var match = regex.Match(subject);

            //if (match.Success) {
            //    Console.WriteLine($"{match.Success}----@{match.Index}----{match.Length}");
            //} else
            //{
            //    Console.WriteLine($"{match.Success}");
            //}
            //    Console.WriteLine(match.Success.ToString());

            while (match.Success)
            {
                Console.WriteLine($"{match.Success}--@{match.Index}--{match.Length}");
                match = match.NextMatch();
            }

            // PAN CARD MATCHING REGEX : [A-Z{5}0-9{4}A-Z{1}]

        }
    }
}
