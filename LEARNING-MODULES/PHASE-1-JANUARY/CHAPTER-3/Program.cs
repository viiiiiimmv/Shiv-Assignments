using System.Text;
namespace CHAPTER_3;

class Program
{
    static void Main(string[] args)
    {
        // string
        string s = "Hello";
        s += "World";
        
        // StringBuilder
        StringBuilder sb = new StringBuilder();
        sb.Append(s);

        // Uppercase
        s.ToUpper();
        
        // Lowercase
        s.ToLower();

        // Trimming
        s = " Hello ";
        s.Trim();
        
        // Substring
        string s1 = "Hello World";
        s1.Substring(1, 5);
        
        // Contains, StartsWith, EndsWith
        string s2 = "Hello World";
        s2.StartsWith('H');
        s2.EndsWith('d');
        s2.Contains("Hello");
        
        // Replace
        string s3 = "I like Java";
        s3.Replace("Java","Kotlin");
        
        // Split
        string str = "A,B,C";
        string[] strs = str.Split(',');
        
        // Join
        string result = string.Join("-", strs);
        
        // IndexOf, LastIndexOf
        string s = "MacBook Air M2";
        s.IndexOf('M');
        s.LastIndexOf('r');
        
        // Equals , ==
        string str4 = "Hello";
        string str5 = "World";
        Console.WriteLine(str4 == str5);
        Console.WriteLine(str4.Equals(str5));
        
        // Empty Strings
        string str1 = String.Empty;
        
        // CompareOrdinal : used to compare strings lexicographically
        str1 = "Hello World";
        string str2 = String.Empty;
        string.CompareOrdinal(str1, str2);
        
        // StringComparison.OrdinalIgnoreCase : enum value that guides C# how to compare two strings
        bool res = string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }
    // Types of Parameters
        
    // Value Parameters :  original value is not effected
    void Increment(int x)
    {
        x++;
    }
    
    // ref parameters : original value can be affected
    void Decrement(ref int x)
    {
        x -= 1;
    }
    
    // out parameters : out is used to return multiple variables
    void ValueSetter(out int x, out int y)
    {
        x = 10;
        y = 20;
    }
    
    // in parameters : used for read only params and not allows modification
    void Print(in int x)
    {
        Console.WriteLine(x);
    }
    
    // Optional / Default parameters
    void Display(int x, int y = 25)
    {
        Console.WriteLine(x + y);
    }
    
    // Named Parameters
    void Information(string name, string id, int age)
    {
        Console.WriteLine($"{name}, {id}, {age}");
    }
    // Information(age:21, name:"Shiv", id:"22BCS15180");
}