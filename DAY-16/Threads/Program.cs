namespace Threads
{
    
    internal class Program
    {
        public static void Func1()
        {
            for(int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Function 1 prints {i}");
            }
        }

        public static void Func2()
        {
            for (int i = 6; i > 0; i--)
            {
                Console.WriteLine($"Function 2 prints {i}");
            }
        }
        static void Main(string[] args)
        {

            ThreadStart t1 = new ThreadStart(Func1);
            ThreadStart t2 = new ThreadStart(Func2);
            Thread first = new Thread(t1);
            Thread second = new Thread(t2);
            first.Start();
            second.Start();

            first.Priority = ThreadPriority.Lowest;
            second.Priority = ThreadPriority.Highest;

            Console.WriteLine();
            Func1();
            Func2();

        }
    }
}
