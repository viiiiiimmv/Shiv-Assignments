namespace DIAGONAL_DIFFERENCE;

class Program
{
    public static int DiagonalDifference(int[][] matrix, int n)
    {
        int diagnol = 0;
        int antiDiagnol = 0;
        for (int i = 0; i < n; i++)
        {
            diagnol += matrix[i][i];
            antiDiagnol += matrix[i][n - 1 - i];
        }
        return Math.Abs(diagnol - antiDiagnol);
    }
    static void Main(string[] args)
    {
        Console.Write("Enter the size of matrix: ");
        int n = Convert.ToInt16(Console.ReadLine());
        int[][] matrix = new int[n][];
        for (int i = 0; i < n; i++)
        {
            matrix[i] = new int[n];
        }
        
        for (int i = 0; i < n; i++)
        {
            for (int j=0; j < n; j++)
            {
                matrix[i][j] = Convert.ToInt32(Console.ReadLine());
            }
        }
        Console.WriteLine($"Absolute diagonal difference: {DiagonalDifference(matrix, n)}");
        
    }
}