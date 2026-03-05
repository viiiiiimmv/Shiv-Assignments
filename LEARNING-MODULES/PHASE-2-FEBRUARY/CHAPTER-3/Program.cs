using System;
using System.Threading;
using System.Threading.Tasks;

namespace CHAPTER_3;

class Program
{
    // * Threading in C#
    static void DoWork()
    {
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"Working {i} - Thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(500);
        }
    }
    
    // * 'async' and 'await' keywords
    static async Task DoAsync()
    {
        await Task.Delay(1000);
        Console.WriteLine($"Main : {Thread.CurrentThread.ManagedThreadId}");
    }

    static async Task Main()
    {
        await DoAsync();
        Console.WriteLine("Done!!!");
    }
    
    static void Main(string[] args)
    {
        // ! Thread Implementation
        Thread t  = new Thread(DoWork);
        t.Start();

        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"Main : {i}");
            Thread.Sleep(1500);
        }
    }
} 