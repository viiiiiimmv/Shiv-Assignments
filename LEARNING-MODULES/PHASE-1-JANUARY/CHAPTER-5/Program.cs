using System.Text.RegularExpressions;
namespace CHAPTER_5;

// * Generic Classes
class Box<T>
{
    public readonly T Value;

    public Box(T value)
    {
        Value = value;
    }
    
    void Swap<t>(ref t a, ref t b)
    {
        (a, b) = (b, a);
    }
}

class Repository<T> where T : class
{
    public void Add(T item)
    {
        Console.WriteLine("Item added");
    }
}

// * Indexers

class StudentDirectory
{
    private Dictionary<int, string> students = new();

    public string this[int id]
    {
        get => students[id];
        set => students[id] = value;
    }
}


// * Static Keyword

// * Static Variable
class Counter
{
    private static int _count = 1;

    public Counter()
    {
        _count++;
    }

    public void Print()
    {
        Console.WriteLine(_count);
    }
}

// * Static Methods
class MathUtils
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}

// * Static Class
// ! Cannot be instantiated and can contain only static methods

static class Utility
{
    public static void SayHello(string name)
    {
        Console.WriteLine($"Hello, {name}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Generic class implementation
        Box<int> box = new Box<int>(1);
        Console.WriteLine($"{box.Value}");
        
        // * Regex Expressions
        bool valid = Regex.IsMatch("abc",@"[a-zA-Z]+");
        
        // * Match() -> Gets the first occurence of target expression
        Match m = Regex.Match("22BCS15180", @"\d+");
        Console.WriteLine($"{m.Value}");
        
        // * Matches() -> Gets the collection of all the matching expressions
        MatchCollection matches = Regex.Matches("a1 b2 c3 d4", @"\d+");
        foreach (Match match in matches)
        {
            Console.Write($"\t{match.Value}");
        }
        
        // * Replace() -> Replaces the occurence of the matching pattern
        string result = Regex.Replace("abc123456789", @"\d+", "#");
        Console.WriteLine($"{result}");
            // .	Any character
            // \d	Digit
            // \D	Non-digit
            // \w	Word (a-z, A-Z, 0-9, _)
            // \W	Non-word
            // \s	Whitespace
            // \S	Non-whitespace
            
        string? input = Console.ReadLine();
        
        // Digits
        Regex.IsMatch(input, @"^\d+$");
        Regex.IsMatch(input, @"^-?\d+$");
        Regex.IsMatch(input, @"^-?\d+(\.\d+)?$");

        // Alphabets / Alphanumeric
        Regex.IsMatch(input, @"^[A-Za-z]+$");
        Regex.IsMatch(input, @"^[A-Za-z ]+$");
        Regex.IsMatch(input, @"^[A-Za-z0-9]+$");

        // Email
        Regex.IsMatch(input, @"^[\w\.-]+@[\w\.-]+\.\w{2,}$");

        // Mobile Numbers
        Regex.IsMatch(input, @"^[6-9]\d{9}$");      // India
        Regex.IsMatch(input, @"^\d{10}$");           // Any 10-digit

        // Passwords
        Regex.IsMatch(input, @"^.{8,}$");
        Regex.IsMatch(input, @"^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!]).{8,}$");

        // Username
        Regex.IsMatch(input, @"^[A-Za-z0-9_]{3,15}$");

        // Dates
        Regex.IsMatch(input, @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$");
        Regex.IsMatch(input, @"^\d{4}-\d{2}-\d{2}$");

        // URL
        Regex.IsMatch(input, @"^(https?:\/\/)?([\w\-]+\.)+[A-Za-z]{2,}\/?$");

        // PAN / Aadhaar
        Regex.IsMatch(input, @"^[A-Z]{5}\d{4}[A-Z]$");
        Regex.IsMatch(input, @"^\d{12}$");

        // Whitespace / Words
        Regex.Replace(input, @"\s+", " ");
        Regex.IsMatch(input, @"\bword\b");

        // File Extensions
        Regex.IsMatch(input, @"\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase);
        Regex.IsMatch(input, @"\.pdf$", RegexOptions.IgnoreCase);

        // Misc
        Regex.IsMatch(input, @"^[A-Za-z].*");
        Regex.IsMatch(input, @".*\d$");
        Regex.IsMatch(input, @"^.{6}$");

        // Extraction
        MatchCollection nums = Regex.Matches(input, @"\d+");
        Match email = Regex.Match(input, @"[\w\.-]+@[\w\.-]+\.\w{2,}");
    }
}