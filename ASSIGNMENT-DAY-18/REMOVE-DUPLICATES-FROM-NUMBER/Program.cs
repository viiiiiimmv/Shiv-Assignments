namespace REMOVE_DUPLICATES_FROM_NUMBER;

class Program
{
    public static int RemovingDuplicates(int num)
    {
        string s = num.ToString();
        HashSet<char> seen = new HashSet<char>();
        string result = "";

        foreach (char c in s)
        {
            if (!seen.Contains(c))
            {
                seen.Add(c);
                result += c;
            }
        }
        return int.Parse(result);
    }
    static void Main(string[] args)
    {
        Console.Write("Enter a number : ");
        int num = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine($"Number after removing duplicates: {RemovingDuplicates(num)}");
    }
}