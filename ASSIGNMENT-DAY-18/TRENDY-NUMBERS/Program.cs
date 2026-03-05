namespace TRENDY_NUMBERS;

class Program
{
    public static bool \IsATrendyNumber(int num)
    {
        if (num < 100 || num > 999) return false;
        
        int mid = (num / 10)%10;
        return (mid % 3 == 0);
    }
    
    static void Main(string[] args)
    {
        Console.Write("Enter a number : ");
        int num = Convert.ToInt32(Console.ReadLine());
        if (IsATrendyNumber(num))
        {
            Console.WriteLine("You entered a Trendy Number");
        }
        else
        {
            Console.WriteLine("You entered a non Trendy number");
        }
    }
}