namespace ThreadSleep;

class Program
{
    static void P()
    {
        for (int i = 1; i <= 20; i++)
        {
            Console.Write('-');
            Thread.Sleep(100);
        }
    }
    static void Main()
    {
        Thread t = new Thread(new ThreadStart(P));
        Console.Write("start");
        t.Start();
        t.Join();
        Console.WriteLine("end");
        Console.ReadLine();
    }
}