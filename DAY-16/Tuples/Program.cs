namespace Tuples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (string Name, int Age, bool IsActive) mike =("Caleb Mclaughlin", 23, true);
            Console.WriteLine($"{mike.Name}\t{mike.Age}\t{mike.IsActive}");
            Console.WriteLine();

            var (name1, age1, isactive1) = mike;

            Console.WriteLine($"Name:{name1}\nAge:{age1}\n"+$"Active:{isactive1}");
            Console.ReadLine();
        }
    }
}
