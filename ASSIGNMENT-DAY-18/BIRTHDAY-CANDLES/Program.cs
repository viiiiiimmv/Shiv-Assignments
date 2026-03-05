namespace BIRTHDAY_CANDLES;

class Program
{
    public static int CountCandles(int[] num)
    {
        int max = int.MinValue;
        foreach (int i in num)
        {
            max = Math.Max(max, i);
        }

        int count = 0;
        foreach (int i in num)
        {
            if (i == max)
            {
                count++;
            }
        }

        return count;
    }
    static void Main(string[] args)
    {
        int[] num = { 1, 2, 3, 3 };
        Console.WriteLine($"Number of tallest candles {CountCandles(num)}");
    }
}