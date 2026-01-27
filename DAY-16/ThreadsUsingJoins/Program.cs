using System;
using System.Threading;

namespace ThreadsUsingJoins;

internal class Program
{

    public static void Fun1()
    {
        for (var i = 1; i < 5; i++)
        {
            Console.WriteLine("Func1() writes {0}", i);
        }

    }

    public static void Fun2()
    {
        for (var j = 6; j > 0; j--)
        {
            Console.WriteLine("Func2() writes {0}", j);
        }

    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Start of the main program ");
        Thread firstThread = new Thread(new ThreadStart(Fun1));
        Thread secondThread = new Thread(new ThreadStart(Fun2));
        firstThread.Start();
        secondThread.Start();
        
        secondThread.Join();
        firstThread.Join();
        Console.WriteLine("\nEnd of main()");
    }
}