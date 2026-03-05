namespace CHAPTER_1;

class Program
{
    static void Main(string[] args)
    {
        // Different data types : int, double, bool, char, string
        int a = 1;
        double b = 2.0;
        bool c = true;
        char d = 'a';
        string e = "Hello World";
        Console.WriteLine($"{a} {b:F2} {c} {d} {e}");
        
        // Boxing
        int f = 10;
        object g = f;
        Console.WriteLine(obj);
        
        // Autoboxing
        object h = 10;
        int i = (int)h;
        Console.WriteLine(i);
        
        // Nullable Datatypes
        int? j = null;
        double? k = null;
        bool? l = null;      
    }
}