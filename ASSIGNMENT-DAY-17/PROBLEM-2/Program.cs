namespace PROBLEM_2;

class Program
{
    public static double VeryBigSum(int N, string[] arr)
    {
        double sum = 0;
        for (int i = 0; i < N; i++)
        {
            sum += double.Parse(arr[i]);
        }
        
        return sum;
    }
    static void Main(string[] args)
    {
        string [] arr = {"1000000001","1000000002","1000000003","1000000004","1000000005","1000000006","1000000007","1000000008","1000000009"};
        Console.WriteLine(VeryBigSum(9, arr));
    }
}