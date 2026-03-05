using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CHAPTER_1;

// * Custom Exception
class InvalidAgeException : Exception
{
    public InvalidAgeException(string message) : base(message) { }
}

// * Finalise (Destructor)
class Shape
{
    public Shape()
    {
        Console.WriteLine("Shape constructor");
    }
    
    ~Shape()
    {
        Console.WriteLine("Shape Destructor");
    }
}

// * Dispose
class FileHandler : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine("Disposed");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // * Exception Handling : try, catch, finally
        int a = 10;
        int b = 0;

        try
        {
            double rem = a/b;
            Console.WriteLine(rem);
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.WriteLine("Finally block reached!!");
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}