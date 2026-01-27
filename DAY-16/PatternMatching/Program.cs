namespace PatternMatching
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your score : ");
            int score = Convert.ToInt32(Console.ReadLine());

            string grade = score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
            Console.WriteLine($"Grade for score {score} is {grade}");
        }
    }
}
