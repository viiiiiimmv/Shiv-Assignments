using System.Diagnostics;

namespace Data_Structures;

class Program
{
    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    public static void InsertionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 1; i < n; i++)
        {
            int key = arr[i];
            int j = i - 1;
            
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = key;
        }
    }

    public static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minidx = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minidx])
                {
                    minidx = j;
                }
            }

            int temp = arr[minidx];
            arr[minidx] = arr[i];
            arr[i] = temp;
        }
    }
    
    public static void CompareSortingEfficiency(int[] data )
    {
        var bubbleSort = data;
        var selectionSort = data;
        var insertSort = data;
        var stopwatch = Stopwatch.StartNew();
        BubbleSort(bubbleSort);
        stopwatch.Stop();
        Console.WriteLine($"Bubble sort Time :{stopwatch.ElapsedMilliseconds} ");

        stopwatch = Stopwatch.StartNew();
        SelectionSort(selectionSort);
        stopwatch.Stop();
        Console.WriteLine($"selection  sort Time :{stopwatch.ElapsedMilliseconds} ");

        stopwatch = Stopwatch.StartNew();
        InsertionSort(insertSort);
        stopwatch.Stop();
        Console.WriteLine($"Insertion  sort Time :{stopwatch.ElapsedMilliseconds} ");
    }

    public static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        return Enumerable.Range(1,size).OrderBy(x => rand.Next()).ToArray();
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}