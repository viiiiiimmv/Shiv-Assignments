namespace LINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[] { 11, 22, 3, 44, 5, 66, 75, 795, 1121 };
            string[] names = new string[] { "John", "Sam", "Peter", "Bucky", "Steve" };

            var greaterThan20 = from number in numbers where number > 20 select number;
            foreach(int num in greaterThan20)
            {
                Console.Write($"{num}\t");
            }
            Console.WriteLine();

            var greaterThan20_2 = numbers.Where(x => x > 20);
            foreach(int num in greaterThan20_2)
            {
                Console.Write($"{num}\t");
            }
            Console.WriteLine();

            var evenNumbersInarr = from number in numbers where number % 2 == 0 select number;
            foreach(int num in evenNumbersInarr)
            {
                Console.Write($"{num}\t");
            }
            Console.WriteLine();

            var evenNumbersInarr_2 = numbers.Where(x => x % 2 == 0);
            foreach (int num in evenNumbersInarr_2)
            {
                Console.Write($"{num}\t");
            }
            Console.WriteLine();

            var sumOfElements = (from number in numbers select number).Sum();
            Console.WriteLine($"The sum of elements is {sumOfElements}");

            var sumOfElements_2 = numbers.Sum();
            Console.WriteLine($"The sum of elements is {sumOfElements_2}");

            var namesWithS = from name in names where name.StartsWith("S") select name;
            Console.WriteLine("Names that start with S :");
            foreach(string name in namesWithS)
            {
                Console.Write($"{name}\t");
            }
            Console.WriteLine();

            Console.WriteLine("Enter a string to find the count of vowels in it : ");
            string inputStr = Console.ReadLine();

            var vowels = inputStr.Where(x => "aeiou".Contains(x));
            var vowelsCount = vowels.Count();
            Console.WriteLine($"Count of vowels : {vowelsCount}");


        }
    }
}
