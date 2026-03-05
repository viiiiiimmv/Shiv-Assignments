namespace EXTRACT_ALPHABETS;

class Program
{
    public static string ExtractingAlphabets(string inputStr)
    {
        List<char> letters = new List<char>();

        foreach (char c in inputStr)
        {
            if (char.IsLetter(c))
            {
                letters.Add(c);
            }
        }

        return string.Join(",", letters);
    }
    static void Main(string[] args)
    {
        Console.Write("Enter a string : ");
        string inputStr = Console.ReadLine();
        string result = ExtractingAlphabets(inputStr);
        Console.WriteLine(result);
    }
}