namespace Record
{
    public record Person(string Name, int Age);
    internal class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person("Caleb Mclaughlin", 23);
            Person p2 = new Person("Sadie Sink", 24);
            Console.WriteLine($"{p1.Name}\t{p1.Age}");
            Console.WriteLine($"{p2.Name}\t{p2.Age}");

            // p1.name = "Peter Parker" ----> Gives error as attributes are immutable in a record
            // Only constructor can be used to set the values

            
        }
    }
}
