namespace PROBLEM_1;

class Program
{
    public static string FindLongestIncreasingTrail(string str)
    {
        int max = 0, curr = 0;
        int cStart = 0, mStart = 0;

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i] > str[i - 1])
            {
                curr++;
            }
            else
            {
                cStart = i;
                curr = 1;
            }

            if (curr > max)
            {
                max = curr;
                mStart = cStart;
            }
        }
        return str.Substring(mStart, max);
    }
    static void Main(string[] args)
    {
        Console.WriteLine(FindLongestIncreasingTrail("DL1CN1234"));
    }
}