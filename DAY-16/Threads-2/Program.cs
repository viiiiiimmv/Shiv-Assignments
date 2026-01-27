using System;
using System.Threading;

namespace Threads_2;

internal class Program
{
    static Thread mainThread;
    static Thread firstThread;
    static Thread secondThread;

    static void Main()
    {
        mainThread = Thread.CurrentThread;
        firstThread = new Thread(new ThreadStart(Fun1));
        secondThread = new Thread(new ThreadStart(Fun2));

        mainThread.Name = "Main Thread";
        firstThread.Name = "First Thread";
        secondThread.Name = "second Thread";

        ThreadsInfo("Main() before starting the threads ");
        firstThread.Start();
        secondThread.Start();
        ThreadsInfo("Main() just before  exiting the Main() method ");
        Console.ReadLine();
    }

    public static void ThreadsInfo(string location)
    {
        Console.WriteLine("\r\n In {0}", location);
        Console.WriteLine("Threads Name :{0}, Thread State : {1}", mainThread.Name, mainThread.ThreadState);
        Console.WriteLine("Threads Name :{0}, Thread State : {1}", firstThread.Name, firstThread.ThreadState);
        Console.WriteLine("Threads Name :{0}, Thread State : {1}", secondThread.Name, secondThread.ThreadState);

    }
    public static void Fun1()
    {
        for (int i = 1; i < 5; i++)
        {
            Console.WriteLine("Func1() writes {0}", i);
            Thread.Sleep(1000);
        }
        ThreadsInfo("Fun1()");
    }

    public static void Fun2()
    {
        for (int j = 6; j > 0; j--)
        {
            Console.WriteLine("Func2() writes {0}", j);
            Thread.Sleep(1250);
        }
        ThreadsInfo("Fun2()");
    }
}