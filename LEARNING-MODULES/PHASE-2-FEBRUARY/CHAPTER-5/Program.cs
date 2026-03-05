namespace CHAPTER_5;

class RecursiveFunctions
{
    public int Factorial(int n)
    {
        if (n == 0) return 1;
        return n * Factorial(n - 1);
    }

    public int Fibonacci(int n)
    {
        if (n == 0) return 0;
        if  (n == 1) return 1;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}